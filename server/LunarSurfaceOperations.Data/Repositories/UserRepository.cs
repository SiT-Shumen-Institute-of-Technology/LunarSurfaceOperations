namespace LunarSurfaceOperations.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Connections.Contracts;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Resources;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository([NotNull] IConnectionManager<IMongoDatabase> databaseConnection, [NotNull] IExhaustiveValidator<User> validator)
            : base(databaseConnection, validator)
        {
        }

        public Task<IOperationResult<User>> GetAsync(ObjectId id, CancellationToken cancellationToken)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            var findOptions = new FindOptions<User>();

            return this.GetAsync(filter, findOptions, cancellationToken);
        }

        public async Task<IOperationResult<User>> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<User>();

            operationResult.ValidateNotNullOrWhitespace(username);
            if (operationResult.Success == false)
                return operationResult;

            try
            {
                var filter = Builders<User>.Filter.Eq(x => x.Username, username);
                var findOptions = new FindOptions<User>();

                var getEntity = await this.GetCollection().FindAsync(filter, findOptions, cancellationToken);
                operationResult.Data = await getEntity.FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception e)
            {
                operationResult.AddException(e);
            }

            return operationResult;
        }

        public async Task<IOperationResult<IEnumerable<User>>> GetManyByUsernameAsync(IEnumerable<string> usernames, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<User>>();

            try
            {
                var filter = Builders<User>.Filter.In(x => x.Username, usernames);
                var findOptions = new FindOptions<User>();

                var getEntity = await this.GetCollection().FindAsync(filter, findOptions, cancellationToken);
                operationResult.Data = await getEntity.ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                operationResult.AddException(e);
            }

            return operationResult;
        }

        public async Task<IOperationResult<IEnumerable<User>>> GetManyAsync(IEnumerable<ObjectId> identifiers, CancellationToken cancellationToken)
        {
            var operationResult = new OperationResult<IEnumerable<User>>();

            try
            {
                var filter = Builders<User>.Filter.In(x => x.Id, identifiers);
                var findOptions = new FindOptions<User>();

                var getEntity = await this.GetCollection().FindAsync(filter, findOptions, cancellationToken);
                operationResult.Data = await getEntity.ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                operationResult.AddException(e);
            }

            return operationResult;
        }

        protected override string CollectionName => CollectionNames.Users;

        protected override IOperationResult HandleModificationException(Exception exception)
        {
            if (exception is MongoWriteException { WriteError: { Category: ServerErrorCategory.DuplicateKey } })
            {
                var operationResult = new OperationResult();
                operationResult.AddErrorMessage(WorkflowMessages.UsernameAlreadyTaken);
                return operationResult;
            }

            return base.HandleModificationException(exception);
        }
    }
}