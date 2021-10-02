namespace LunarSurfaceOperations.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using MongoDB.Bson;

    public interface IMessageRepository : IRepository<Message>
    {
        Task<IOperationResult<IEnumerable<Message>>> GetManyAsync(ObjectId workspaceId, CancellationToken cancellationToken);
        Task<IOperationResult<Message>> GetAsync(ObjectId workspaceId, ObjectId id, CancellationToken cancellationToken);
    }
}