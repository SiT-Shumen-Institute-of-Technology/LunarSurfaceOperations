namespace LunarSurfaceOperations.Core.Contracts.Services
{
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;

    public interface IWorkspaceService : IBaseService<IWorkspacePrototype, IWorkspaceLayout>
    {
    }
}