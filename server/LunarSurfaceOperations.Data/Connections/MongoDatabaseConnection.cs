namespace LunarSurfaceOperations.Data.Connections
{
    using System;
    using LunarSurfaceOperations.Configuration.Database;
    using LunarSurfaceOperations.Connections.Contracts;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;

    public class MongoDatabaseConnection : IConnectionManager<IMongoDatabase>
    {
        public MongoDatabaseConnection(IOptions<DatabaseSettings> databaseSettings)
        {
            var actualSettings = databaseSettings?.Value;
            if (actualSettings is null)
                throw new ArgumentNullException(nameof(databaseSettings));

            var mongoClient = new MongoClient(actualSettings.ConnectionString);
            this.Connection = mongoClient.GetDatabase(actualSettings.DatabaseName);
        }

        public IMongoDatabase Connection { get; }
    }
}