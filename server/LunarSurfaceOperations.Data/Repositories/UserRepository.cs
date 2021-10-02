namespace LunarSurfaceOperations.Data.Repositories
{
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Connections.Contracts;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Validation.Contracts;
    using MongoDB.Driver;

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository([NotNull] IConnectionManager<IMongoDatabase> databaseConnection, [NotNull] IExhaustiveValidator<User> validator) 
            : base(databaseConnection, validator)
        {
        }

        protected override string CollectionName => "Users";
    }
}
