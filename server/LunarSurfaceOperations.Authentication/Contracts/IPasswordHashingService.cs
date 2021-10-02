namespace LunarSurfaceOperations.Authentication.Contracts
{
    using LunarSurfaceOperations.Authentication.OperativeModels;
    using LunarSurfaceOperations.Utilities.OperationResults;

    /// <summary>
    /// An interface defining the structure of a component that should be responsible for the password management operations.
    /// </summary>
    public interface IPasswordHashingService
    {
        /// <summary>
        /// Use this method to hash the requested text.
        /// </summary>
        /// <param name="text">The text that should be hashed.</param>
        /// <returns>Returns an <see cref="OperationResult{T}"/> representing the operation.</returns>
        IOperationResult<HashResult> Hash(string text);
        
        /// <summary>
        /// Use this method to validate whether or not the requested <paramref name="input"/> corresponds to the expected hash value.
        /// </summary>
        /// <param name="input">The input that should be validated.</param>
        /// <param name="expectedHash">The previously generated hash value.</param>
        /// <param name="usedSalt">The `salt` value that is internally used to create unique hashes for the same input.</param>
        /// <returns>Returns true if the input is correct. Else, returns false.</returns>
        bool Validate(string input, string expectedHash, string usedSalt);
    }
}