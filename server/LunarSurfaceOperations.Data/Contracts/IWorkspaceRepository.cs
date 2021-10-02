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
        Task<IOperationResult<IEnumerable<Workspace>>> GetForUserAsync(ObjectId userId, CancellationToken cancellationToken);
    }
}
