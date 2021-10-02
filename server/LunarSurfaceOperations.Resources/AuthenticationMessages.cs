namespace LunarSurfaceOperations.Resources
{
    using System.Resources;

    public static class AuthenticationMessages
    {
        private static readonly ResourceManager _resourceManager = ResourceManagerBuilder.CreateResourceManager("AuthenticationMessages");

        public static string MissingUser => _resourceManager.GetString(nameof(MissingUser));
        public static string InvalidPassword => _resourceManager.GetString(nameof(InvalidPassword));
    }
}