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
    using Quantum.DMS.Utilities;

    public class WorkspaceRepository : BaseRepository<Workspace>, IWorkspaceRepository
    {
        public WorkspaceRepository([NotNull] IConnectionManager<IMongoDatabase> databaseConnection, [NotNull] IExhaustiveValidator<Workspace> validator)
            : base(databaseConnection, validator)
        {
        }

        public async Task<IOperationResult<IEnumerable<Workspace>>> GetByUserAsync(ObjectId userId, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<Workspace>>();

            try
            {
                var filter = Builders<Workspace>.Filter.Or(Builders<Workspace>.Filter.AnyEq(x => x.Members, userId), Builders<Workspace>.Filter.Eq(x => x.OwnerId, userId));
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

        public async Task<IOperationResult> UpdateMembers(ObjectId workspaceId, IEnumerable<ObjectId> members, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult();

            var filter = Builders<Workspace>.Filter.Eq(x => x.Id, workspaceId);
            var updateDefinition = Builders<Workspace>.Update.Set(x => x.Members, members.OrEmptyIfNull().IgnoreDefaultValues());
            var updateOptions = new UpdateOptions();

            var updateResult = await this.UpdateAsync(filter, updateDefinition, updateOptions, cancellationToken);
            if (updateResult.Success == false)
                operationResult.AppendErrorMessages(updateResult);

            return operationResult;
        }

        protected override string CollectionName => CollectionNames.Workspaces;
    }
}