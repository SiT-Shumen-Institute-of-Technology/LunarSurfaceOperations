namespace LunarSurfaceOperations.Data.Repositories
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Connections.Contracts;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Utilities.OperationResults;
    using LunarSurfaceOperations.Validation.Contracts;
    using MongoDB.Driver;

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository([NotNull] IConnectionManager<IMongoDatabase> databaseConnection, [NotNull] IExhaustiveValidator<User> validator)
            : base(databaseConnection, validator)
        {
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

        protected override string CollectionName => "Users";
    }
}