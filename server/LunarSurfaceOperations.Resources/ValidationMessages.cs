namespace LunarSurfaceOperations.Resources
{
    using System.Resources;

    public static class ValidationMessages
    {
        private static readonly ResourceManager _resourceManager = ResourceManagerBuilder.CreateResourceManager("ValidationMessages");

        public static string InvalidNullArgument => _resourceManager.GetString(nameof(InvalidNullArgument));
        public static string InvalidStringArgument => _resourceManager.GetString(nameof(InvalidStringArgument));
    }
}