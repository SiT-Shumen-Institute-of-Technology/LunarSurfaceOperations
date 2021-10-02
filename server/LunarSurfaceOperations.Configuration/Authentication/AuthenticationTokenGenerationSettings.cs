namespace LunarSurfaceOperations.Configuration.Authentication
{
    using JetBrains.Annotations;

    public class AuthenticationTokenGenerationSettings
    {
        public const string Section = "AuthTokenGeneration";

        public string Secret { get; [UsedImplicitly] set; }

        public int Expiry { get; [UsedImplicitly] set; }

        public string Issuer { get; [UsedImplicitly] set; }

        public string Audience { get; [UsedImplicitly] set; }
    }
}