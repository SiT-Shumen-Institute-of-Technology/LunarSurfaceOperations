namespace LunarSurfaceOperations.Core.Services
{
    using System;
    using System.Collections.Generic;
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
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;
    using MongoDB.Bson;

    public class MessageService : BaseService<IMessageRepository, Message, WorkspaceScopeIdentification<Message>, IMessagePrototype, IMessageLayout>, IMessageService
    {
        private readonly IWorkspaceService _workspaceService;
        private readonly IAuthenticationContext _authenticationContext;
        
        public MessageService([NotNull] IMessageRepository repository, [NotNull] IExhaustiveValidator<IMessagePrototype> validator, [NotNull] IWorkspaceService workspaceService, [NotNull] IAuthenticationContext authenticationContext)
            : base(repository, validator)
        {
            this._workspaceService = workspaceService ?? throw new ArgumentNullException(nameof(workspaceService));
            this._authenticationContext = authenticationContext ?? throw new ArgumentNullException(nameof(authenticationContext));
        }

        public async Task<IOperationResult<IEnumerable<IMessageLayout>>> GetManyAsync(ObjectId workspaceId, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<IMessageLayout>>();

            var getMessages = await this.Repository.GetManyAsync(workspaceId, cancellationToken);
            if (getMessages.Success is false)
                return operationResult.AppendErrorMessages(getMessages);

            var constructLayouts = this.ConstructManyLayouts(getMessages.Data);
            if (constructLayouts.Success is false)
                return operationResult.AppendErrorMessages(constructLayouts);

            operationResult.Data = constructLayouts.Data;
            return operationResult;
        }

        public Task<IOperationResult<IMessageLayout>> CreateAsync(ObjectId workspaceId, IMessagePrototype prototype, CancellationToken cancellationToken)
            => this.CreateInternallyAsync(new WorkspaceScopeIdentification<Message>(workspaceId), prototype, cancellationToken);

        protected override IOperationResult EnhanceDatabaseModel(Message databaseModel, IMessagePrototype prototype)
        {
            var operationResult = new OperationResult();

            operationResult.ValidateNotNull(databaseModel);
            operationResult.ValidateNotNull(prototype);

            if (operationResult.Success is false)
                return operationResult;

            databaseModel.Text = prototype.Text;
            databaseModel.AuthorId = this._authenticationContext.CurrentUser.Id;
            databaseModel.Timestamp = DateTime.Now;
            
            return operationResult;
        }

        protected override IOperationResult<IMessageLayout> ConstructLayout(Message entity)
        {
            var operationResult = new OperationResult<IMessageLayout>();

            operationResult.ValidateNotNull(entity);
            if (operationResult.Success is false)
                return operationResult;

            operationResult.Data = new MessageLayout(entity.Id, entity.WorkspaceId, entity.Text);
            return operationResult;
        }

        protected override async Task<IOperationResult<Message>> GetEntityByIdAsync(ObjectId entityId, WorkspaceScopeIdentification<Message> identification, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<Message>();

            operationResult.ValidateNotNull(identification);
            if (operationResult.Success is false)
                return operationResult;

            var workspaceId = identification.WorkspaceId;
            var validateAccessibleWorkspace = await this._workspaceService.ValidateIsAccessibleAsync(workspaceId, cancellationToken);
            if (validateAccessibleWorkspace.Success is false)
                return operationResult.AppendErrorMessages(validateAccessibleWorkspace);

            var getEntity = await this.Repository.GetAsync(workspaceId, entityId, cancellationToken);
            if (getEntity.Success is false)
                return operationResult.AppendErrorMessages(getEntity);

            operationResult.Data = getEntity.Data;
            return operationResult;
        }
    }
}