namespace LunarSurfaceOperations.API.ViewModels.Workspace
{
    using JetBrains.Annotations;

    public class WorkspaceViewModel
    {
        public string Id { [UsedImplicitly] get; init; }
        public string Name { [UsedImplicitly] get; init; }
        public string Description { [UsedImplicitly] get; init; }
    }
}