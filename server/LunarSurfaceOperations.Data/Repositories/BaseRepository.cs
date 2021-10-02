namespace LunarSurfaceOperations.Data.Repositories
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Connections.Contracts;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Resources;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly IConnectionManager<IMongoDatabase> _databaseConnection;
        private readonly IExhaustiveValidator<TEntity> _validator;

        protected BaseRepository([NotNull] IConnectionManager<IMongoDatabase> databaseConnection, [NotNull] IExhaustiveValidator<TEntity> validator)
        {
            this._databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
            this._validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        protected abstract string CollectionName { get; }

        public async Task<IOperationResult<TEntity>> GetAsync(ObjectId id, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<TEntity>();

            try
            {
                var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
                var findOptions = new FindOptions<TEntity>();

                var getEntity = await this.GetCollection().FindAsync(filter, findOptions, cancellationToken);
                operationResult.Data = await getEntity.FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception e)
            {
                operationResult.AddException(e);
            }

            return operationResult;
        }

        public async Task<IOperationResult> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult();

            operationResult.ValidateNotNull(entity);
            if (operationResult.Success is false)
                return operationResult;

            var validationResult = await this._validator.ValidateAsync(entity, cancellationToken);
            if (validationResult.Success is false)
                return operationResult.AppendErrorMessages(validationResult);

            try
            {
                var insertOptions = new InsertOneOptions();
                await this.GetCollection().InsertOneAsync(entity, insertOptions, cancellationToken);
            }
            catch (Exception e)
            {
                operationResult.AppendErrorMessages(this.HandleModificationException(e));
            }

            return operationResult;
        }

        public async Task<IOperationResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult();

            operationResult.ValidateNotNull(entity);
            if (operationResult.Success is false)
                return operationResult;

            var validationResult = await this._validator.ValidateAsync(entity, cancellationToken);
            if (validationResult.Success is false)
                return operationResult.AppendErrorMessages(validationResult);

            try
            {
                var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
                var replaceOptions = new ReplaceOptions();

                var replaceOneResult = await this.GetCollection().ReplaceOneAsync(filter, entity, replaceOptions, cancellationToken);
                if (replaceOneResult.MatchedCount != 1)
                    operationResult.AddErrorMessage(WorkflowMessages.UpdateHasNoMatches);
            }
            catch (Exception e)
            {
                operationResult.AppendErrorMessages(this.HandleModificationException(e));
            }

            return operationResult;
        }

        protected async Task<IOperationResult> UpdateAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition, UpdateOptions options, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult();
            
            operationResult.ValidateNotNull(filter);
            operationResult.ValidateNotNull(updateDefinition);
            
            try
            {
                var replaceOneResult = await this.GetCollection().UpdateOneAsync(filter, updateDefinition, options, cancellationToken);
                if (replaceOneResult.MatchedCount != 1)
                    operationResult.AddErrorMessage(WorkflowMessages.UpdateHasNoMatches);
            }
            catch (Exception e)
            {
                operationResult.AppendErrorMessages(this.HandleModificationException(e));
            }

            return operationResult;
        }

        protected virtual IOperationResult HandleModificationException(Exception exception)
        {
            var operationResult = new OperationResult();

            if (exception is not null)
                operationResult.AddException(exception);

            return operationResult;
        }

        protected IMongoCollection<TEntity> GetCollection() => this._databaseConnection.Connection.GetCollection<TEntity>(this.CollectionName);
    }
}