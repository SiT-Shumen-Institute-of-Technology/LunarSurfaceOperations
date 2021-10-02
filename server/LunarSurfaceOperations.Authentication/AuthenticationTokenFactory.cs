namespace LunarSurfaceOperations.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using LunarSurfaceOperations.Authentication.Contracts;
    using LunarSurfaceOperations.Configuration.Authentication;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Quantum.DMS.Utilities;

    public class AuthenticationTokenFactory : IAuthenticationTokenFactory
    {
        private readonly IOptions<AuthenticationTokenGenerationSettings> _options;

        public AuthenticationTokenFactory(IOptions<AuthenticationTokenGenerationSettings> options)
        {
            this._options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public IOperationResult<string> GenerateToken(IDictionary<string, string> claims)
        {
            var operationResult = new OperationResult<string>();

            var settings = this._options?.Value;
            var secret = settings?.Secret;
            operationResult.ValidateNotNullOrWhitespace(secret);
            if (operationResult.Success is false || string.IsNullOrWhiteSpace(secret))
                return operationResult;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwtHeader = new JwtHeader(credentials);

            var tokenClaims = new List<Claim>(claims.Count);
            foreach (var (key, value) in claims.OrEmptyIfNull())
            {
                var currentClaim = new Claim(key, value);
                tokenClaims.Add(currentClaim);
            }

            // NOTE: Tony Troeff, 02/08/2021 - In future, we may think about the subject and actor public claims.
            var jwtPayload = new JwtPayload(
                issuer: settings.Issuer,
                audience: settings.Audience,
                claims: tokenClaims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(settings.Expiry),
                issuedAt: DateTime.Now);

            var token = new JwtSecurityToken(jwtHeader, jwtPayload);

            var tokenHandler = new JwtSecurityTokenHandler();
            operationResult.Data = tokenHandler.WriteToken(token);

            return operationResult;
        }
    }
}