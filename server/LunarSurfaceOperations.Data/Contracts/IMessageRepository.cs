namespace LunarSurfaceOperations.Data.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using MongoDB.Bson;

    public interface IMessageRepository : IRepository<Message>
    {
        Task<IOperationResult<Message>> GetAsync(ObjectId workspaceId, ObjectId id, CancellationToken cancellationToken);
    }
}