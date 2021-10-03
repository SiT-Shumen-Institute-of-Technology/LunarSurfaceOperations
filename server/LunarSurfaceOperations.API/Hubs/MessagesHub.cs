namespace LunarSurfaceOperations.API.Hubs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using Microsoft.AspNetCore.SignalR;

    public class MessagesHub : BaseHub<IMessageHubClient>
    {
        [NotNull]
        private readonly IWorkspaceService _workspaceService;

        public MessagesHub([NotNull] IWorkspaceService workspaceService)
        {
            this._workspaceService = workspaceService ?? throw new ArgumentNullException(nameof(workspaceService));
        }

        public async Task ConnectToWorkspace(string workspaceId)
        {
            var workspaceEntityId = ConvertToId(workspaceId);
            
            var validateAccessibleWorkspace = await this._workspaceService.ValidateIsAccessibleAsync(workspaceEntityId, this.GetCancellationToken());
            if (validateAccessibleWorkspace.Success is false)
                throw new HubException(validateAccessibleWorkspace.ToString());

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, workspaceId, this.GetCancellationToken());
        }

        public async Task DisconnectFromWorkspace(string workspaceId) 
            => await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, workspaceId, this.GetCancellationToken());

        private CancellationToken GetCancellationToken() => this.Context.ConnectionAborted;
    }
}
