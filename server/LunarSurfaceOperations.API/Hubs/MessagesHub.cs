namespace LunarSurfaceOperations.API.Hubs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using Microsoft.AspNetCore.SignalR;
    using MongoDB.Bson;

    public class MessagesHub : Hub<IMessageHubClient>
    {
        [NotNull]
        private readonly IWorkspaceService _workspaceService;

        public MessagesHub([NotNull] IWorkspaceService workspaceService)
        {
            this._workspaceService = workspaceService ?? throw new ArgumentNullException(nameof(workspaceService));
        }

        public async Task ConnectToWorkspace(ObjectId workspaceId, CancellationToken cancellationToken)
        {
            var validateAccessibleWorkspace = await this._workspaceService.ValidateIsAccessibleAsync(workspaceId, cancellationToken);
            if (validateAccessibleWorkspace.Success is false)
                throw new HubException(validateAccessibleWorkspace.ToString());

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, workspaceId.ToString(), cancellationToken);
        }

        public async Task DisconnectFromWorkspace(ObjectId workspaceId, CancellationToken cancellationToken) => await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, workspaceId.ToString(), cancellationToken);
    }
}