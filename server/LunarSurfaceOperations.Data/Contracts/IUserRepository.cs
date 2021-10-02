namespace LunarSurfaceOperations.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using MongoDB.Bson;

    public interface IUserRepository : IRepository<User>
    {
        Task<IOperationResult<IEnumerable<User>>> GetManyAsync(IEnumerable<ObjectId> identifiers, CancellationToken cancellationToken);
        Task<IOperationResult<User>> GetByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<IOperationResult<IEnumerable<User>>> GetManyByUsernameAsync(IEnumerable<string> username, CancellationToken cancellationToken);
    }
}
