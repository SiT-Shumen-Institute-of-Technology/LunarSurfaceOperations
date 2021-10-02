namespace LunarSurfaceOperations.API.ViewModels.Workspace
{
    using JetBrains.Annotations;

    public class WorkspaceViewModel
    {
        public string Id { [UsedImplicitly] get; set; }
        public string Name { [UsedImplicitly] get; set; }
        public string Description { [UsedImplicitly] get; set; }
    }
}