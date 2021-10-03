namespace LunarSurfaceOperations.API.Hubs
{
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;

    public interface IWorkspaceHubClient
    {
        Task InviteToWorkspace(IWorkspaceLayout workspace);
        Task RemoveFromWorkspace(string workspaceId);
    }
}