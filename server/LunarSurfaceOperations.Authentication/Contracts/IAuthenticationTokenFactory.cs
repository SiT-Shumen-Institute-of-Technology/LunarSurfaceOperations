namespace LunarSurfaceOperations.Authentication.Contracts
{
    using System.Collections.Generic;
    using LunarSurfaceOperations.Utilities.OperationResults;

    /// <summary>
    /// An interface defining the structure of a component that should be responsible of generating authentication tokens.
    /// </summary>
    public interface IAuthenticationTokenFactory
    {
        /// <summary>
        /// Use this method to generate an authentication token according to the requested <paramref name="claims"/>.
        /// </summary>
        /// <param name="claims">The claims that should be used during the authentication token generation.</param>
        /// <returns>Returns an <see cref="OperationResult"/> representing the operation.</returns>
        IOperationResult<string> GenerateToken(IDictionary<string, string> claims);
    }
}