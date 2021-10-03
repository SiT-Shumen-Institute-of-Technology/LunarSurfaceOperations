namespace LunarSurfaceOperations.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.API.Factories.Contracts;
    using LunarSurfaceOperations.API.Hubs;
    using LunarSurfaceOperations.API.ViewModels.Workspace;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Resources;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using MongoDB.Bson;
    using Quantum.DMS.Utilities;

    [ApiController]
    [Route("_workspaces")]
    public class WorkspacesController : ControllerBase
    {
        [NotNull]
        private readonly IWorkspaceService _workspaceService;

        [NotNull]
        private readonly IWorkspaceFactory _workspaceFactory;

        [NotNull]
        private readonly IUserFactory _userFactory;

        [NotNull]
        private readonly IHubContext<WorkspacesHub, IWorkspaceHubClient> _workspacesHubContext;

        public WorkspacesController(
            [NotNull] IWorkspaceService workspaceService,
            [NotNull] IWorkspaceFactory workspaceFactory,
            [NotNull] IUserFactory userFactory,
            [NotNull] IHubContext<WorkspacesHub, IWorkspaceHubClient> workspacesHubContext)
        {
            this._workspaceService = workspaceService ?? throw new ArgumentNullException(nameof(workspaceService));
            this._workspaceFactory = workspaceFactory ?? throw new ArgumentNullException(nameof(workspaceFactory));
            this._userFactory = userFactory ?? throw new ArgumentNullException(nameof(userFactory));
            this._workspacesHubContext = workspacesHubContext ?? throw new ArgumentNullException(nameof(workspacesHubContext));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] WorkspaceInputModel inputModel, CancellationToken cancellationToken)
        {
            if (inputModel is null)
                return this.BadRequest(ValidationMessages.InvalidRequest);

            var workspacePrototype = new WorkspacePrototype(inputModel.Name, inputModel.Description);
            var createWorkspace = await this._workspaceService.CreateAsync(workspacePrototype, cancellationToken);
            if (createWorkspace.Success is false)
                return this.BadRequest(createWorkspace.ToString());

            return this.Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var getWorkspaces = await this._workspaceService.GetAllAsync(cancellationToken);
            if (getWorkspaces.Success == false)
                return this.Problem(getWorkspaces.ToString());

            var viewModels = new List<WorkspaceViewModel>();

            foreach (var workspaceLayout in getWorkspaces.Data.OrEmptyIfNull().IgnoreNullValues())
            {
                var viewModel = this._workspaceFactory.ToViewModel(workspaceLayout);
                viewModels.Add(viewModel);
            }

            return this.Ok(viewModels);
        }

        [HttpPut("members")]
        public async Task<IActionResult> UpdateMembersAsync([FromQuery] ObjectId id, [FromBody] UpdateWorkspaceMembersRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
                return this.BadRequest(ValidationMessages.InvalidRequest);

            var updateMembers = await this._workspaceService.UpdateMembersAsync(id, request.Members.OrEmptyIfNull().IgnoreNullValues(), cancellationToken);
            if (updateMembers.Success == false)
                return this.BadRequest(updateMembers.ToString());

            var updateMembersLayout = updateMembers.Data;
            if (updateMembersLayout is null)
                return this.BadRequest(ValidationMessages.InvalidRequest);

            var newUsers = updateMembersLayout.NewUsers.OrEmptyIfNull().IgnoreDefaultValues().Select(x => x.ToString()).ToHashSet();
            var oldUsers = updateMembersLayout.RemovedUsers.OrEmptyIfNull().IgnoreDefaultValues().Select(x => x.ToString()).ToHashSet();

            if (newUsers.Any())
                await this._workspacesHubContext.Clients.Users(newUsers).InviteToWorkspace(updateMembersLayout.Workspace);
            if (oldUsers.Any())
                await this._workspacesHubContext.Clients.Users(oldUsers).RemoveFromWorkspace(updateMembersLayout.Workspace.Id.ToString());

            return this.Ok();
        }

        [HttpGet("members")]
        public async Task<IActionResult> GetMembersAsync([FromQuery] ObjectId id, CancellationToken cancellationToken)
        {
            var getMembers = await this._workspaceService.GetMembersAsync(id, cancellationToken);
            if (getMembers.Success is false)
                return this.BadRequest(getMembers.ToString());

            return this.Ok(getMembers.Data.OrEmptyIfNull().IgnoreNullValues().Select(this._userFactory.ToViewModel));
        }
    }
}