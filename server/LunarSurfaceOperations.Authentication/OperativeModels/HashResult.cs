namespace LunarSurfaceOperations.Authentication.OperativeModels
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// An operative model that should represent the result of text hashing.
    /// </summary>
    public class HashResult
    {
        /// <summary>
        /// Gets the generated hash value.
        /// </summary>
        [NotNull]
        public string HashedValue { get; }
        
        /// <summary>
        /// Gets the `salt` value that is internally used to create unique hashes for the same input.
        /// </summary>
        [NotNull]
        public string Salt { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashResult"/> class.
        /// </summary>
        /// <param name="hashedValue">The value that should be set to the <see cref="HashedValue"/> property.</param>
        /// <param name="salt">The value that should be set to the <see cref="Salt"/> property.</param>
        public HashResult([NotNull] string hashedValue, [NotNull] string salt)
        {
            this.HashedValue = hashedValue ?? throw new ArgumentNullException(nameof(hashedValue));
            this.Salt = salt ?? throw new ArgumentNullException(nameof(salt));
        }
    }
}