namespace LunarSurfaceOperations.Core.Contracts.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Authentication;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using MongoDB.Bson;

    public interface IUserService
    {
        Task<OperationResult<IUserLayout>> GetAsync(ObjectId userId, CancellationToken cancellationToken);
        Task<IOperationResult<IAuthenticationData>> GetAuthenticationDataAsync(string username, string password, CancellationToken cancellationToken);
        Task<OperationResult<IEnumerable<IUserLayout>>> GetManyByUsernameAsync(IEnumerable<string> usernames, CancellationToken cancellationToken);
        Task<OperationResult<IEnumerable<IUserLayout>>> GetManyAsync(IEnumerable<ObjectId> identifiers, CancellationToken cancellationToken);
        
        Task<IOperationResult<IUserLayout>> CreateAsync(IUserPrototype prototype, CancellationToken cancellationToken);
        Task<IOperationResult<IUserLayout>> UpdateAsync(ObjectId id, IUserPrototype prototype, CancellationToken cancellationToken);
    }
}