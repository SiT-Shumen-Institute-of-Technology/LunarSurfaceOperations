namespace LunarSurfaceOperations.Data.Models
{
    using LunarSurfaceOperations.Data.Contracts;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class BaseEntity : IEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }
    }
}