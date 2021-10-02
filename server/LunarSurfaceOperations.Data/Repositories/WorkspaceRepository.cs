namespace LunarSurfaceOperations.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Connections.Contracts;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class WorkspaceRepository : BaseRepository<Workspace>, IWorkspaceRepository
    {
        public WorkspaceRepository([NotNull] IConnectionManager<IMongoDatabase> databaseConnection, [NotNull] IExhaustiveValidator<Workspace> validator) 
            : base(databaseConnection, validator)
        {
        }

        public async Task<IOperationResult<IEnumerable<Workspace>>> GetForUserAsync(ObjectId userId, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<Workspace>>();
            
            try
            {
                var filter = Builders<Workspace>.Filter.Eq(x => x.OwnerId, userId);
                var findOptions = new FindOptions<Workspace>();

                var getEntity = await this.GetCollection().FindAsync(filter, findOptions, cancellationToken);
                operationResult.Data = await getEntity.ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                operationResult.AddException(e);
            }

            return operationResult;
        }

        protected override string CollectionName => CollectionNames.Workspaces;
    }
}
