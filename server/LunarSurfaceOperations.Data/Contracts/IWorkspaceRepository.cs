namespace LunarSurfaceOperations.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using MongoDB.Bson;

    public interface IWorkspaceRepository : IRepository<Workspace>
    {
        Task<IOperationResult<Workspace>> GetAsync(ObjectId id, CancellationToken cancellationToken);
        Task<IOperationResult<IEnumerable<Workspace>>> GetByUserAsync(ObjectId userId, CancellationToken cancellationToken);
    }
}
