namespace LunarSurfaceOperations.API.Factories
{
    using LunarSurfaceOperations.API.Factories.Contracts;
    using LunarSurfaceOperations.API.ViewModels.Workspace;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;

    public class WorkspaceFactory : IWorkspaceFactory
    {
        public WorkspaceViewModel ToViewModel(IWorkspaceLayout workspaceLayout)
        {
            if (workspaceLayout is null)
                return null;
            
            return new WorkspaceViewModel { Id = workspaceLayout.Id, Name = workspaceLayout.Name, Description = workspaceLayout.Description };
        }
    }
}