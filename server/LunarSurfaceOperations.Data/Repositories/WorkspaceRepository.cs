namespace LunarSurfaceOperations.Data.Repositories
{
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Connections.Contracts;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Validation.Contracts;
    using MongoDB.Driver;

    public class WorkspaceRepository : BaseRepository<Workspace>, IWorkspaceRepository
    {
        public WorkspaceRepository([NotNull] IConnectionManager<IMongoDatabase> databaseConnection, [NotNull] IExhaustiveValidator<Workspace> validator) 
            : base(databaseConnection, validator)
        {
        }

        protected override string CollectionName => "Workspaces";
    }
}
