namespace LunarSurfaceOperations.Core.Contracts.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using MongoDB.Bson;

    public interface IMessageService
    {
        Task<IOperationResult<IEnumerable<IMessageLayout>>> GetManyAsync(ObjectId workspaceId, CancellationToken cancellationToken);
        Task<IOperationResult<IMessageLayout>> CreateAsync(ObjectId workspaceId, IMessagePrototype prototype, CancellationToken cancellationToken);
        Task<IOperationResult> ApproveAsync(ObjectId workspaceId, ObjectId messageId, CancellationToken cancellationToken);
    }
}