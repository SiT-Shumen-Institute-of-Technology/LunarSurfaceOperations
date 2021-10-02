namespace LunarSurfaceOperations.Data.Repositories
{
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

    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository([NotNull] IConnectionManager<IMongoDatabase> databaseConnection, [NotNull] IExhaustiveValidator<Message> validator)
            : base(databaseConnection, validator)
        {
        }

        public Task<IOperationResult<Message>> GetAsync(ObjectId workspaceId, ObjectId id, CancellationToken cancellationToken)
        {
            var filters = Builders<Message>.Filter.And(
                Builders<Message>.Filter.Eq(m => m.WorkspaceId, workspaceId),
                Builders<Message>.Filter.Eq(m => m.Id, id));
            var findOptions = new FindOptions<Message>();
            return this.GetAsync(filters, findOptions, cancellationToken);
        }
        
        protected override string CollectionName => CollectionNames.Messages;
    }
}