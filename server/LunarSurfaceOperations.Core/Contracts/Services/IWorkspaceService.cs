namespace LunarSurfaceOperations.Core.Contracts.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Utilities.OperationResults;

    public interface IWorkspaceService : IBaseService<IWorkspacePrototype, IWorkspaceLayout>
    {
        Task<IOperationResult<IEnumerable<IWorkspaceLayout>>> GetAllAsync(CancellationToken cancellationToken);
    }
}