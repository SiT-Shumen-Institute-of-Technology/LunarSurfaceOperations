namespace LunarSurfaceOperations.API.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.API.ViewModels.Workspace;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Resources;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("_workspaces")]
    public class WorkspacesController : ControllerBase
    {
        [NotNull]
        private readonly IWorkspaceService _workspaceService;

        public WorkspacesController([NotNull] IWorkspaceService workspaceService)
        {
            this._workspaceService = workspaceService ?? throw new ArgumentNullException(nameof(workspaceService));
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
    }
}