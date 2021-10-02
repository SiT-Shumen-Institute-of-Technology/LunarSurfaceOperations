namespace LunarSurfaceOperations.Core.Services.ScopeIdentification
{
    using LunarSurfaceOperations.Data.Contracts;
    using MongoDB.Bson;

    public class WorkspaceScopeIdentification<TEntity> : IScopeIdentification<TEntity>
        where TEntity : IWorkspaceDependentEntity
    {
        public ObjectId WorkspaceId { get; }

        public WorkspaceScopeIdentification(ObjectId workspaceId)
        {
            this.WorkspaceId = workspaceId;
        }

        public void Apply(TEntity entity)
        {
            if (entity is null)
                return;

            entity.WorkspaceId = this.WorkspaceId;
        }
    }
}