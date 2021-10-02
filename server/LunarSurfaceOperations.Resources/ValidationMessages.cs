namespace LunarSurfaceOperations.Resources
{
    using System.Reflection;
    using System.Resources;
    using JetBrains.Annotations;

    public static class ValidationMessages
    {
        private static readonly ResourceManager _resourceManager = ResourceManagerBuilder.CreateResourceManager("ValidationMessages");
        public static string InvalidNullArgument => _resourceManager.GetString(nameof(InvalidNullArgument));
        public static string NotEqual => _resourceManager.GetString(nameof(NotEqual));
        public static string InvalidLength => _resourceManager.GetString(nameof(InvalidLength));
    }
}