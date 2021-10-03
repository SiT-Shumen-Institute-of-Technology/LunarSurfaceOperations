namespace LunarSurfaceOperations.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Core.Contracts.Authentication;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Services.ScopeIdentification;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Resources;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;
    using MongoDB.Bson;
    using Quantum.DMS.Utilities;

    public class WorkspaceService : BaseService<IWorkspaceRepository, Workspace, EmptyScopeIdentification<Workspace>, IWorkspacePrototype, IWorkspaceLayout>, IWorkspaceService
    {
        private readonly IAuthenticationContext _authenticationContext;
        private readonly IUserService _userService;

        public WorkspaceService(
            [NotNull] IWorkspaceRepository repository,
            [NotNull] IExhaustiveValidator<IWorkspacePrototype> validator,
            [NotNull] IAuthenticationContext authenticationContext,
            [NotNull] IUserService userService)
            : base(repository, validator)
        {
            this._authenticationContext = authenticationContext ?? throw new ArgumentNullException(nameof(authenticationContext));
            this._userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<IOperationResult<IEnumerable<IWorkspaceLayout>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<IWorkspaceLayout>>();

            var getWorkspaces = await this.Repository.GetByUserAsync(this._authenticationContext.CurrentUser.Id, cancellationToken);
            if (getWorkspaces.Success is false)
                return operationResult.AppendErrorMessages(getWorkspaces);

            var layouts = new List<IWorkspaceLayout>();
            foreach (var workspace in getWorkspaces.Data.OrEmptyIfNull().IgnoreNullValues())
            {
                var constructLayout = await this.ConstructLayout(workspace, cancellationToken);
                if (constructLayout.Success is false)
                    return operationResult.AppendErrorMessages(constructLayout);

                var layout = constructLayout.Data;
                if (layout is not null)
                    layouts.Add(layout);
            }

            operationResult.Data = layouts;
            return operationResult;
        }

        public async Task<IOperationResult> UpdateMembersAsync(ObjectId workspaceId, IEnumerable<string> members, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult();

            var getWorkspace = await this.GetEntityInternallyAsync(workspaceId, cancellationToken);
            if (getWorkspace.Success is false)
                return operationResult.AppendErrorMessages(getWorkspace);

            var workspace = getWorkspace.Data;
            operationResult.ValidateNotNull(workspace, WorkflowMessages.EntityNotFound);
            if (operationResult.Success is false)
                return operationResult;

            var currentUser = this._authenticationContext.CurrentUser;
            if (workspace.OwnerId != currentUser.Id)
            {
                operationResult.AddErrorMessage(WorkflowMessages.UserIsNotOwnerOfWorkspace);
                return operationResult;
            }

            var getInvitedUsers = await this._userService.GetManyByUsernameAsync(members, cancellationToken);
            if (getInvitedUsers.Success is false)
                return operationResult.AppendErrorMessages(getInvitedUsers);

            var invitedUsersIdentifiers = getInvitedUsers.Data.OrEmptyIfNull().IgnoreNullValues().Select(u => u.Id);
            var updateWorkspace = await this.Repository.UpdateMembers(workspaceId, invitedUsersIdentifiers, cancellationToken);
            if (updateWorkspace.Success is false)
                operationResult.AppendErrorMessages(updateWorkspace);

            return operationResult;
        }

        public async Task<IOperationResult<IEnumerable<IUserLayout>>> GetMembersAsync(ObjectId workspaceId, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<IUserLayout>>();

            var getWorkspace = await this.GetEntityInternallyAsync(workspaceId, cancellationToken);
            if (getWorkspace.Success is false)
                return operationResult.AppendErrorMessages(getWorkspace);

            var workspace = getWorkspace.Data;
            operationResult.ValidateNotNull(workspace, WorkflowMessages.EntityNotFound);
            if (operationResult.Success is false)
                return operationResult;

            var allMembers = workspace.OwnerId.AsEnumerable().ConcatenateWith(workspace.Members);
            var getUsers = await this._userService.GetManyAsync(allMembers, cancellationToken);
            if (getUsers.Success is false)
                return operationResult.AppendErrorMessages(getUsers);

            operationResult.Data = getUsers.Data;
            return operationResult;
        }

        public async Task<IOperationResult> ValidateIsAccessibleAsync(ObjectId workspaceId, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<bool>();

            var getWorkspace = await this.GetEntityInternallyAsync(workspaceId, cancellationToken);
            if (getWorkspace.Success is false)
                return operationResult.AppendErrorMessages(getWorkspace);

            var workspace = getWorkspace.Data;
            operationResult.ValidateNotNull(workspace, WorkflowMessages.EntityNotFound);
            return operationResult;
        }

        public Task<IOperationResult<IWorkspaceLayout>> CreateAsync(IWorkspacePrototype prototype, CancellationToken cancellationToken)
            => this.CreateInternallyAsync(new EmptyScopeIdentification<Workspace>(), prototype, cancellationToken);

        public Task<IOperationResult<IWorkspaceLayout>> UpdateAsync(ObjectId id, IWorkspacePrototype prototype, CancellationToken cancellationToken)
            => this.UpdateInternallyAsync(id, new EmptyScopeIdentification<Workspace>(), prototype, cancellationToken);

        protected override IOperationResult EnhanceDatabaseModel(Workspace entity, IWorkspacePrototype prototype)
        {
            var operationResult = new OperationResult();

            operationResult.ValidateNotNull(entity);
            operationResult.ValidateNotNull(prototype);
            if (operationResult.Success == false)
                return operationResult;

            entity.Name = prototype.Name;
            entity.Description = prototype.Description;
            entity.OwnerId = this._authenticationContext.CurrentUser.Id;
            return operationResult;
        }

        protected override Task<IOperationResult<IWorkspaceLayout>> ConstructLayout(Workspace entity, CancellationToken cancellationToken) => Task.FromResult(this.ConstructLayoutInternally(entity));

        private IOperationResult<IWorkspaceLayout> ConstructLayoutInternally(Workspace entity)
        {
            var operationResult = new OperationResult<IWorkspaceLayout>();

            operationResult.ValidateNotNull(entity);
            if (operationResult.Success == false)
                return operationResult;

            operationResult.Data = new WorkspaceLayout(entity.Id, entity.Name, entity.Description);
            return operationResult;
        }

        protected override Task<IOperationResult<Workspace>> GetEntityInternallyAsync(ObjectId entityId, EmptyScopeIdentification<Workspace> identification, CancellationToken cancellationToken)
            => this.GetEntityInternallyAsync(entityId, cancellationToken);

        private async Task<IOperationResult<Workspace>> GetEntityInternallyAsync(ObjectId entityId, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<Workspace>();

            var getWorkspace = await this.Repository.GetAsync(entityId, cancellationToken);
            if (getWorkspace.Success is false)
                return operationResult.AppendErrorMessages(getWorkspace);

            var workspace = getWorkspace.Data;
            if (workspace is null)
                return operationResult;

            var currentUserId = this._authenticationContext.CurrentUser.Id;
            if (workspace.OwnerId != currentUserId && workspace.Members.Contains(currentUserId) is false)
                operationResult.AddErrorMessage(WorkflowMessages.UserIsNotMemberOfWorkspace);
            else
                operationResult.Data = workspace;

            return operationResult;
        }
    }
}