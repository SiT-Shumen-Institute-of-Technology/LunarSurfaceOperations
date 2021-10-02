namespace LunarSurfaceOperations.Data.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class BaseEntity
    {
        [BsonId]
        public ObjectId ObjectId { get; set; }
        
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }
    }
}