namespace LunarSurfaceOperations.API.ViewModels.Workspace
{
    using JetBrains.Annotations;
    using LunarSurfaceOperations.API.ViewModels.User;
    using MongoDB.Bson;

    public class WorkspaceViewModel
    {
        public ObjectId Id { [UsedImplicitly] get; init; }
        public string Name { [UsedImplicitly] get; init; }
        public string Description { [UsedImplicitly] get; init; }
        public UserViewModel Owner { [UsedImplicitly] get; init; }
    }
}