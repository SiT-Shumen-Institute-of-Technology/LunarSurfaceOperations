namespace LunarSurfaceOperations.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.API.Factories.Contracts;
    using LunarSurfaceOperations.API.ViewModels.Workspace;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Resources;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Bson;
    using Quantum.DMS.Utilities;

    [ApiController]
    [Route("_workspaces")]
    public class WorkspacesController : ControllerBase
    {
        [NotNull]
        private readonly IWorkspaceService _workspaceService;

        [NotNull]
        private readonly IUserFactory _userFactory;

        public WorkspacesController([NotNull] IWorkspaceService workspaceService, [NotNull] IUserFactory userFactory)
        {
            this._workspaceService = workspaceService ?? throw new ArgumentNullException(nameof(workspaceService));
            this._userFactory = userFactory ?? throw new ArgumentNullException(nameof(userFactory));
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
                var viewModel = new WorkspaceViewModel { Id = workspaceLayout.Id.ToString(), Name = workspaceLayout.Name, Description = workspaceLayout.Description };
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