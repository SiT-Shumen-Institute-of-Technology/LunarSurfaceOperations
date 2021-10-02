namespace LunarSurfaceOperations.Data.Contracts
{
    using MongoDB.Bson;

    public interface IWorkspaceDependentEntity
    {
        ObjectId WorkspaceId { get; set; }
    }
}