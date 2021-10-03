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
    using LunarSurfaceOperations.Core.Contracts.Processors.MessageAttribute;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Services.ScopeIdentification;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Data.Enums;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Resources;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;
    using MongoDB.Bson;
    using Quantum.DMS.Utilities;

    public class MessageService : BaseService<IMessageRepository, Message, WorkspaceScopeIdentification<Message>, IMessagePrototype, IMessageLayout>, IMessageService
    {
        private readonly IWorkspaceService _workspaceService;
        private readonly IUserService _userService;
        private readonly IAuthenticationContext _authenticationContext;
        private readonly IReadOnlyCollection<IMessageAttributeProcessor> _attributeProcessors;

        public MessageService(
            [NotNull] IMessageRepository repository,
            [NotNull] IExhaustiveValidator<IMessagePrototype> validator,
            [NotNull] IWorkspaceService workspaceService,
            [NotNull] IUserService userService,
            [NotNull] IAuthenticationContext authenticationContext,
            [CanBeNull] IEnumerable<IMessageAttributeProcessor> attributeProcessors)
            : base(repository, validator)
        {
            this._workspaceService = workspaceService ?? throw new ArgumentNullException(nameof(workspaceService));
            this._userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this._authenticationContext = authenticationContext ?? throw new ArgumentNullException(nameof(authenticationContext));
            this._attributeProcessors = attributeProcessors.OrEmptyIfNull().IgnoreNullValues().ToList().AsReadOnly();
        }

        public async Task<IOperationResult<IEnumerable<IMessageLayout>>> GetManyAsync(ObjectId workspaceId, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<IMessageLayout>>();

            var getMessages = await this.Repository.GetManyAsync(workspaceId, cancellationToken);
            if (getMessages.Success is false)
                return operationResult.AppendErrorMessages(getMessages);

            var constructLayouts = await this.ConstructManyLayouts(getMessages.Data, cancellationToken);
            if (constructLayouts.Success is false)
                return operationResult.AppendErrorMessages(constructLayouts);

            operationResult.Data = constructLayouts.Data;
            return operationResult;
        }

        public Task<IOperationResult<IMessageLayout>> CreateAsync(ObjectId workspaceId, IMessagePrototype prototype, CancellationToken cancellationToken)
            => this.CreateInternallyAsync(new WorkspaceScopeIdentification<Message>(workspaceId), prototype, cancellationToken);

        public async Task<IOperationResult<IMessageLayout>> ApproveAsync(ObjectId workspaceId, ObjectId messageId, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IMessageLayout>();

            var workspaceIdentification = new WorkspaceScopeIdentification<Message>(workspaceId);
            var getMessage = await this.GetEntityInternallyAsync(messageId, workspaceIdentification, cancellationToken);
            if (getMessage.Success is false)
                return operationResult.AppendErrorMessages(getMessage);

            var message = getMessage.Data;
            operationResult.ValidateNotNull(message, WorkflowMessages.EntityNotFound);
            if (operationResult.Success is false)
                return operationResult;

            var currentUserId = this._authenticationContext.CurrentUser.Id;
            if (message.AuthorId != currentUserId)
            {
                operationResult.AddErrorMessage(WorkflowMessages.UserIsNotAuthorOfMessage);
                return operationResult;
            }

            message.Status = MessageStatus.OfficiallyApproved;
            var updateStatus = await this.Repository.UpdateAsync(message, cancellationToken);
            if (updateStatus.Success is false)
                operationResult.AppendErrorMessages(updateStatus);

            var constructLayout = await this.ConstructLayout(message, cancellationToken);
            if (constructLayout.Success is false)
                return operationResult.AppendErrorMessages(constructLayout);

            operationResult.Data = constructLayout.Data;
            return operationResult;
        }

        protected override IOperationResult EnhanceDatabaseModel(Message entity, IMessagePrototype prototype)
        {
            var operationResult = new OperationResult();

            operationResult.ValidateNotNull(entity);
            operationResult.ValidateNotNull(prototype);

            if (operationResult.Success is false)
                return operationResult;

            entity.Text = prototype.Text;
            entity.AuthorId = this._authenticationContext.CurrentUser.Id;
            entity.Timestamp = DateTime.Now;
            entity.Status = MessageStatus.Unofficial;

            foreach (var attributePrototype in prototype.Attributes.OrEmptyIfNull().IgnoreNullValues())
            {
                var materializedAttribute = attributePrototype.Materialize();
                if (materializedAttribute is not null)
                    entity.Attributes.Add(materializedAttribute);
            }

            return operationResult;
        }

        protected override async Task<IOperationResult<IEnumerable<IMessageLayout>>> ConstructManyLayouts(IEnumerable<Message> entities, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<IMessageLayout>>();

            var layouts = new List<IMessageLayout>();
            operationResult.Data = layouts;
            
            var iteratedEntities = entities.OrEmptyIfNull().IgnoreNullValues().ToList().AsReadOnly();
            if (iteratedEntities.Any() is false)
                return operationResult;

            var userIdentifiers = iteratedEntities.Select(m => m.AuthorId);
            var getUsers = await this._userService.GetManyAsync(userIdentifiers, cancellationToken);
            if (getUsers.Success is false)
                return operationResult.AppendErrorMessages(getUsers);

            var organizedUsers = new Dictionary<ObjectId, IUserLayout>();
            foreach (var userLayout in getUsers.Data.OrEmptyIfNull().IgnoreNullValues())
                organizedUsers[userLayout.Id] = userLayout;

            foreach (var message in iteratedEntities)
            {
                organizedUsers.TryGetValue(message.AuthorId, out var author);
                var constructMessageLayout = await this.ConstructLayoutInternally(message, author, cancellationToken);
                if (constructMessageLayout.Success is false)
                    return operationResult.AppendErrorMessages(constructMessageLayout);

                var constructedLayout = constructMessageLayout.Data;
                if (constructedLayout is not null)
                    layouts.Add(constructedLayout);
            }

            return operationResult;
        }

        protected override async Task<IOperationResult<IMessageLayout>> ConstructLayout(Message entity, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IMessageLayout>();

            operationResult.ValidateNotNull(entity);
            if (operationResult.Success is false)
                return operationResult;

            var getAuthor = await this._userService.GetAsync(entity.AuthorId, cancellationToken).ConfigureAwait(false);
            if (getAuthor.Success is false)
                return operationResult.AppendErrorMessages(getAuthor);

            var constructLayout = await this.ConstructLayoutInternally(entity, getAuthor.Data, cancellationToken);
            if (constructLayout.Success is false)
                return operationResult.AppendErrorMessages(constructLayout);

            operationResult.Data = constructLayout.Data;
            return operationResult;
        }

        private async Task<IOperationResult<IMessageLayout>> ConstructLayoutInternally(Message message, IUserLayout author, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IMessageLayout>();
            
            operationResult.ValidateNotNull(message);
            operationResult.ValidateNotNull(author);
            if (operationResult.Success is false)
                return operationResult;
            
            var messageLayout = new MessageLayout(message.Id, message.WorkspaceId, message.Text, author, message.Timestamp, message.Status);
            foreach (var messageAttribute in message.Attributes.OrEmptyIfNull().IgnoreNullValues())
            {
                if (this.TryProcessAttribute(messageAttribute, out var intermediaryProcessor) == false)
                    continue;

                var constructAttributeLayout = await intermediaryProcessor.ConstructLayoutAsync(cancellationToken);
                if (constructAttributeLayout.Success is false)
                    return operationResult.AppendErrorMessages(constructAttributeLayout);

                var attributeLayout = constructAttributeLayout.Data;
                if (attributeLayout is not null)
                    messageLayout.AddAttribute(attributeLayout);
            }

            operationResult.Data = messageLayout;
            return operationResult;
        }

        private bool TryProcessAttribute(IMessageAttribute attribute, out IMessageAttributeIntermediaryProcessor intermediaryProcessor)
        {
            intermediaryProcessor = null;
            if (attribute is null)
                return false;

            foreach (var attributeProcessor in this._attributeProcessors)
            {
                if (attributeProcessor.CanProcess(attribute) is false)
                    continue;

                intermediaryProcessor = attributeProcessor.Process(attribute);
                return intermediaryProcessor is not null;
            }

            return false;
        }

        protected override async Task<IOperationResult<Message>> GetEntityInternallyAsync(ObjectId entityId, WorkspaceScopeIdentification<Message> identification, CancellationToken cancellationToken)
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

        protected override bool CanBeEdited(Message entity)
        {
            if (entity is null || entity.Status == MessageStatus.OfficiallyApproved)
                return false;
            return base.CanBeEdited(entity);
        }
    }
}