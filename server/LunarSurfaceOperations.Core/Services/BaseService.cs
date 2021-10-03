namespace LunarSurfaceOperations.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Services.ScopeIdentification;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Resources;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;
    using MongoDB.Bson;
    using Quantum.DMS.Utilities;

    public abstract class BaseService<TRepository, TEntity, TScopeIdentification, TPrototype, TLayout>
        where TRepository : IRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TScopeIdentification : class, IScopeIdentification<TEntity>
        where TPrototype : class
        where TLayout : class, ILayout
    {
        protected BaseService([NotNull] TRepository repository, [NotNull] IExhaustiveValidator<TPrototype> validator)
        {
            this.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.Validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [NotNull]
        protected TRepository Repository { get; }

        [NotNull]
        protected IExhaustiveValidator<TPrototype> Validator { get; }

        protected async Task<IOperationResult<TLayout>> CreateInternallyAsync(TScopeIdentification identification, TPrototype prototype, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<TLayout>();

            operationResult.ValidateNotNull(identification);
            operationResult.ValidateNotNull(prototype);
            if (operationResult.Success is false)
                return operationResult;

            var validationResult = await this.Validator.ValidateAsync(prototype, cancellationToken);
            if (validationResult.Success is false)
                return operationResult.AppendErrorMessages(validationResult);

            var databaseModel = new TEntity { Id = ObjectId.GenerateNewId() };
            identification.Apply(databaseModel);

            var enhanceDatabaseModel = this.EnhanceDatabaseModel(databaseModel, prototype);
            if (enhanceDatabaseModel.Success is false)
                return operationResult.AppendErrorMessages(enhanceDatabaseModel);

            var createResult = await this.Repository.CreateAsync(databaseModel, cancellationToken);
            if (createResult.Success is false)
                return operationResult.AppendErrorMessages(createResult);

            var constructLayout = await this.ConstructLayout(databaseModel, cancellationToken);
            if (constructLayout.Success is false)
                return operationResult.AppendErrorMessages(constructLayout);

            operationResult.Data = constructLayout.Data;
            return operationResult;
        }

        protected async Task<IOperationResult<TLayout>> UpdateInternallyAsync(ObjectId id, TScopeIdentification identification, TPrototype prototype, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<TLayout>();

            operationResult.ValidateNotNull(prototype);
            operationResult.ValidateNotNull(identification);
            if (operationResult.Success is false)
                return operationResult;

            var validationResult = await this.Validator.ValidateAsync(prototype, cancellationToken);
            if (validationResult.Success is false)
                return operationResult.AppendErrorMessages(validationResult);

            var getEntity = await this.GetEntityInternallyAsync(id, identification, cancellationToken);
            if (getEntity.Success is false)
                return operationResult.AppendErrorMessages(getEntity);

            var originalEntity = getEntity.Data;
            if (originalEntity is null)
                operationResult.AddErrorMessage(WorkflowMessages.UpdateHasNoMatches);
            else if (this.CanBeEdited(originalEntity) is false)
                operationResult.AddErrorMessage(WorkflowMessages.EntityCannotBeUpdated);

            if (operationResult.Success is false)
                return operationResult;

            var enhanceDatabaseModel = this.EnhanceDatabaseModel(originalEntity, prototype);
            if (enhanceDatabaseModel.Success is false)
                return operationResult.AppendErrorMessages(enhanceDatabaseModel);

            var updateResult = await this.Repository.UpdateAsync(originalEntity, cancellationToken);
            if (updateResult.Success is false)
                operationResult.AppendErrorMessages(updateResult);

            var constructLayout = await this.ConstructLayout(originalEntity, cancellationToken);
            if (constructLayout.Success is false)
                return operationResult.AppendErrorMessages(constructLayout);

            operationResult.Data = constructLayout.Data;
            return operationResult;
        }

        protected async Task<IOperationResult<IEnumerable<TLayout>>> ConstructManyLayouts(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<TLayout>>();

            var layouts = new List<TLayout>();
            foreach (var entity in entities.OrEmptyIfNull().IgnoreNullValues())
            {
                var constructLayout = await this.ConstructLayout(entity, cancellationToken);
                if (constructLayout.Success is false)
                    return operationResult.AppendErrorMessages(constructLayout);

                var layout = constructLayout.Data;
                if (layout is not null)
                    layouts.Add(layout);
            }

            operationResult.Data = layouts;
            return operationResult;
        }

        protected virtual bool CanBeEdited(TEntity entity) => true;

        protected abstract IOperationResult EnhanceDatabaseModel(TEntity entity, TPrototype prototype);

        protected abstract Task<IOperationResult<TLayout>> ConstructLayout(TEntity entity, CancellationToken cancellationToken);

        protected abstract Task<IOperationResult<TEntity>> GetEntityInternallyAsync(ObjectId entityId, TScopeIdentification identification, CancellationToken cancellationToken);
    }
}