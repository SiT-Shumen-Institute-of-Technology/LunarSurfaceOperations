namespace LunarSurfaceOperations.API.Factories.Contracts
{
    using LunarSurfaceOperations.API.ViewModels.Workspace;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;

    public interface IWorkspaceFactory
    {
        WorkspaceViewModel ToViewModel(IWorkspaceLayout workspaceLayout);
    }
}