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

        public async Task<IOperationResult<IUpdateWorkspaceMembersLayout>> UpdateMembersAsync(ObjectId workspaceId, IEnumerable<string> members, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IUpdateWorkspaceMembersLayout>();

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

            var oldMembers = workspace.Members.ToHashSet();
            var newMembers = new HashSet<ObjectId>();
            workspace.Members.Clear();
            foreach (var userLayout in getInvitedUsers.Data.OrEmptyIfNull().IgnoreNullValues())
            {
                newMembers.Add(userLayout.Id);
                workspace.Members.Add(userLayout.Id);
            }

            var updateWorkspace = await this.Repository.UpdateAsync(workspace, cancellationToken);
            if (updateWorkspace.Success is false)
                return operationResult.AppendErrorMessages(updateWorkspace);

            var constructLayout = await this.ConstructLayout(workspace, cancellationToken);
            if (constructLayout.Success is false)
                return operationResult.AppendErrorMessages(constructLayout);

            var updateMembersLayout = new UpdateWorkspaceMembersLayout(constructLayout.Data);
            foreach (var oldMember in oldMembers)
                if (newMembers.Contains(oldMember) is false)
                    updateMembersLayout.AddRemovedUser(oldMember);
            foreach (var newMember in newMembers)
                if (oldMembers.Contains(newMember) is false)
                    updateMembersLayout.AddNewUser(newMember);

            operationResult.Data = updateMembersLayout;
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
        
        protected override async Task<IOperationResult<IEnumerable<IWorkspaceLayout>>> ConstructManyLayouts(IEnumerable<Workspace> entities, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<IWorkspaceLayout>>();

            var layouts = new List<IWorkspaceLayout>();
            operationResult.Data = layouts;
            
            var iteratedEntities = entities.OrEmptyIfNull().IgnoreNullValues().ToList().AsReadOnly();
            if (iteratedEntities.Any() is false)
                return operationResult;

            var userIdentifiers = iteratedEntities.Select(m => m.OwnerId);
            var getUsers = await this._userService.GetManyAsync(userIdentifiers, cancellationToken);
            if (getUsers.Success is false)
                return operationResult.AppendErrorMessages(getUsers);

            var organizedUsers = new Dictionary<ObjectId, IUserLayout>();
            foreach (var userLayout in getUsers.Data.OrEmptyIfNull().IgnoreNullValues())
                organizedUsers[userLayout.Id] = userLayout;

            foreach (var message in iteratedEntities)
            {
                organizedUsers.TryGetValue(message.OwnerId, out var author);
                var constructMessageLayout = this.ConstructLayoutInternally(message, author);
                if (constructMessageLayout.Success is false)
                    return operationResult.AppendErrorMessages(constructMessageLayout);

                var constructedLayout = constructMessageLayout.Data;
                if (constructedLayout is not null)
                    layouts.Add(constructedLayout);
            }

            return operationResult;
        }

        protected override async Task<IOperationResult<IWorkspaceLayout>> ConstructLayout(Workspace entity, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IWorkspaceLayout>();

            operationResult.ValidateNotNull(entity);
            if (operationResult.Success == false)
                return operationResult;

            var getAuthor = await this._userService.GetAsync(entity.OwnerId, cancellationToken).ConfigureAwait(false);
            if (getAuthor.Success is false)
                return operationResult.AppendErrorMessages(getAuthor);

            var constructLayout = this.ConstructLayoutInternally(entity, getAuthor.Data);
            if (constructLayout.Success is false)
                return operationResult.AppendErrorMessages(constructLayout);

            operationResult.Data = constructLayout.Data;
            return operationResult;
        }

        private IOperationResult<IWorkspaceLayout> ConstructLayoutInternally(Workspace entity, IUserLayout owner)
        {
            var operationResult = new OperationResult<IWorkspaceLayout>();

            operationResult.ValidateNotNull(entity);
            operationResult.ValidateNotNull(owner);
            if (operationResult.Success == false)
                return operationResult;

            operationResult.Data = new WorkspaceLayout(entity.Id, entity.Name, entity.Description, owner);
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