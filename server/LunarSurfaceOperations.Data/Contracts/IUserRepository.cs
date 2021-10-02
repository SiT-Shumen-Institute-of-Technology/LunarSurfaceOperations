namespace LunarSurfaceOperations.Data.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Utilities.OperationResults;

    public interface IUserRepository : IRepository<User>
    {
        Task<IOperationResult<User>> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    }
}
