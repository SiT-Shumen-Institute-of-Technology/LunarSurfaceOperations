namespace LunarSurfaceOperations.Configuration.Authentication
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public class AuthenticationTokenValidationSettings
    {
        public const string Section = "AuthTokenValidation";

        public IEnumerable<string> IssuerSigningKeys { get; [UsedImplicitly] set; }

        public IEnumerable<string> Issuers { get; [UsedImplicitly] set; }

        public IEnumerable<string> Audiences { get; [UsedImplicitly] set; }

        public IEnumerable<string> DecryptionKeys { get; [UsedImplicitly] set; }
    }
}