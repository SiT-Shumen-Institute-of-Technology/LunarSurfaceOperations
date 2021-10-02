namespace LunarSurfaceOperations.API.ViewModels.Workspace
{
    using JetBrains.Annotations;

    public class WorkspaceInputModel
    {
        public string Name { get; [UsedImplicitly] set; }
        public string Description { get; [UsedImplicitly] set; }
    }
}