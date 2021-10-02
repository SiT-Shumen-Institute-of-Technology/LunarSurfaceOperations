namespace LunarSurfaceOperations.Authentication
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using LunarSurfaceOperations.Authentication.Contracts;
    using LunarSurfaceOperations.Authentication.OperativeModels;
    using LunarSurfaceOperations.Utilities.OperationResults;

    /// <summary>
    /// A default implementation of the <see cref="IPasswordHashingService"/> interface.
    /// It will internally use SHA-512 hashing.
    /// </summary>
    public class PasswordHashingService : IPasswordHashingService
    {
        private const byte SaltLength = 32;

        /// <inheritdoc />
        public IOperationResult<HashResult> Hash(string text)
        {
            var operationResult = new OperationResult<HashResult>();

            operationResult.ValidateNotNullOrWhitespace(text);
            if (operationResult.Success == false)
                return operationResult;

            var saltBytes = GenerateSalt();
            var hash = GenerateHash(text, saltBytes);
            var salt = AsString(saltBytes);
            operationResult.Data = new HashResult(hash, salt);

            return operationResult;
        }

        /// <inheritdoc />
        public bool Validate(string input, string expectedHash, string usedSalt)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(expectedHash) || string.IsNullOrWhiteSpace(usedSalt))
                return false;

            try
            {
                var saltBuffer = AsBuffer(usedSalt);
                var generatedHash = GenerateHash(input, saltBuffer);
                return string.Equals(generatedHash, expectedHash);
            }
            catch
            {
                return false;
            }
        }

        private static byte[] GenerateSalt()
        {
            using var randomNumbersGenerator = new RNGCryptoServiceProvider();
            var saltBytes = new byte[SaltLength];
            randomNumbersGenerator.GetBytes(saltBytes);
            return saltBytes;
        }

        private static string GenerateHash(string input, byte[] salt)
        {
            var textBytes = Encoding.UTF8.GetBytes(input);
            var totalInput = new byte[salt.Length + textBytes.Length];
            Buffer.BlockCopy(textBytes, 0, totalInput, 0, textBytes.Length);
            Buffer.BlockCopy(salt, 0, totalInput, textBytes.Length, salt.Length);

            using var sha512CryptoServiceProvider = new SHA512CryptoServiceProvider();
            var computedHashBytes = sha512CryptoServiceProvider.ComputeHash(totalInput);
            return AsString(computedHashBytes);
        }

        private static byte[] AsBuffer(string text) => Convert.FromBase64String(text);

        private static string AsString(byte[] buffer) => Convert.ToBase64String(buffer);
    }
}