namespace LunarSurfaceOperations.Core.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Resources;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;
    using MongoDB.Bson;

    public abstract class BaseService<TRepository, TEntity, TPrototype, TLayout> : IBaseService<TPrototype, TLayout>
        where TRepository : IRepository<TEntity>
        where TEntity : class, IEntity, new()
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

        public async Task<IOperationResult<TLayout>> CreateAsync(TPrototype prototype, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<TLayout>();

            operationResult.ValidateNotNull(prototype);
            if (operationResult.Success is false)
                return operationResult;

            var validationResult = await this.Validator.ValidateAsync(prototype, cancellationToken);
            if (validationResult.Success is false)
                return operationResult.AppendErrorMessages(validationResult);

            var databaseModel = new TEntity { Id = ObjectId.GenerateNewId() };

            var enhanceDatabaseModel = this.EnhanceDatabaseModel(databaseModel, prototype);
            if (enhanceDatabaseModel.Success is false)
                return operationResult.AppendErrorMessages(enhanceDatabaseModel);

            var createResult = await this.Repository.CreateAsync(databaseModel, cancellationToken);
            if (createResult.Success is false)
                return operationResult.AppendErrorMessages(createResult);

            var constructLayout = this.ConstructLayout(databaseModel);
            if (constructLayout.Success is false)
                return operationResult.AppendErrorMessages(constructLayout);

            operationResult.Data = constructLayout.Data;
            return operationResult;
        }

        public async Task<IOperationResult<TLayout>> UpdateAsync(ObjectId id, TPrototype prototype, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<TLayout>();

            operationResult.ValidateNotNull(prototype);
            if (operationResult.Success is false)
                return operationResult;

            var validationResult = await this.Validator.ValidateAsync(prototype, cancellationToken);
            if (validationResult.Success is false)
                return operationResult.AppendErrorMessages(validationResult);

            var getEntity = await this.Repository.GetAsync(id, cancellationToken);
            if (getEntity.Success is false)
                return operationResult.AppendErrorMessages(getEntity);

            var originalEntity = getEntity.Data;
            if (originalEntity is null)
            {
                operationResult.AddErrorMessage(WorkflowMessages.UpdateHasNoMatches);
                return operationResult;
            }

            var enhanceDatabaseModel = this.EnhanceDatabaseModel(originalEntity, prototype);
            if (enhanceDatabaseModel.Success is false)
                return operationResult.AppendErrorMessages(enhanceDatabaseModel);

            var createResult = await this.Repository.UpdateAsync(originalEntity, cancellationToken);
            if (createResult.Success is false)
                operationResult.AppendErrorMessages(createResult);

            var constructLayout = this.ConstructLayout(originalEntity);
            if (constructLayout.Success is false)
                return operationResult.AppendErrorMessages(constructLayout);

            operationResult.Data = constructLayout.Data;
            return operationResult;
            
        }

        protected abstract IOperationResult EnhanceDatabaseModel(TEntity databaseModel, TPrototype prototype);

        protected abstract IOperationResult<TLayout> ConstructLayout(TEntity entity);
    }
}