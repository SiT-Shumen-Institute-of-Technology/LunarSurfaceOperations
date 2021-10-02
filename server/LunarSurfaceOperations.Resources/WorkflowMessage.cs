namespace LunarSurfaceOperations.Resources
{
    using System.Resources;

    public static class WorkflowMessages
    {
        private static readonly ResourceManager _resourceManager = ResourceManagerBuilder.CreateResourceManager("WorkflowMessages");

        public static string UpdateHasNoMatches => _resourceManager.GetString(nameof(UpdateHasNoMatches));
    }
}