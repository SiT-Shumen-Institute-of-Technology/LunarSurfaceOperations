namespace LunarSurfaceOperations.Core.Contracts.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Authentication;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Utilities.OperationResults;

    public interface IUserService : IBaseService<IUserPrototype, IUserLayout>
    {
        Task<IOperationResult<IAuthenticationData>> GetAuthenticationDataAsync(string username, string password, CancellationToken cancellationToken);
    }
}