namespace LunarSurfaceOperations.Data.Models
{
    using MongoDB.Bson.Serialization.Attributes;

    public class Workspace : BaseEntity
    {
        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("d")]
        [BsonIgnoreIfDefault]
        public string Description { get; set; }
    }
}