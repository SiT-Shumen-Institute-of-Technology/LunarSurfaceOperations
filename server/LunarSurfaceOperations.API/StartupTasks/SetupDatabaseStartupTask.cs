namespace LunarSurfaceOperations.API.StartupTasks
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Connections.Contracts;
    using LunarSurfaceOperations.Data;
    using LunarSurfaceOperations.Data.Models;
    using MongoDB.Driver;

    public class SetupDatabaseStartupTask : IStartupTask
    {
        [NotNull]
        private readonly IConnectionManager<IMongoDatabase> _databaseConnection;

        public SetupDatabaseStartupTask([NotNull] IConnectionManager<IMongoDatabase> databaseConnection)
        {
            this._databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var database = this._databaseConnection.Connection;

            var listCollectionNamesOptions = new ListCollectionNamesOptions();
            var collectionsCursor = await database.ListCollectionNamesAsync(listCollectionNamesOptions, cancellationToken);

            var collections = await collectionsCursor.ToListAsync(cancellationToken);
            var collectionsSet = new HashSet<string>(collections);

            if (collectionsSet.Contains(CollectionNames.Users) is false)
            {
                await database.CreateCollectionAsync(CollectionNames.Users, new CreateCollectionOptions(), cancellationToken);
                var usersCollection = database.GetCollection<User>(CollectionNames.Users);

                var usernameIndexKey = Builders<User>.IndexKeys.Ascending(u => u.Username);
                var createIndexModel = new CreateIndexModel<User>(usernameIndexKey, new CreateIndexOptions { Unique = true });
                await usersCollection.Indexes.CreateOneAsync(createIndexModel, new CreateOneIndexOptions(), cancellationToken);
            }

            if (collectionsSet.Contains(CollectionNames.Workspaces) is false)
            {
                await database.CreateCollectionAsync(CollectionNames.Workspaces, new CreateCollectionOptions(), cancellationToken);
            }
        }
    }
}