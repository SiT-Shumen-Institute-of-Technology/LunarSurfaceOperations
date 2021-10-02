namespace LunarSurfaceOperations.Data.Models
{
    using MongoDB.Bson.Serialization.Attributes;

    public class Workspaces : BaseEntity
    {
        [BsonElement("n")]
        public string Name { get; set; }
        
        [BsonElement("d")]
        [BsonIgnoreIfDefault]
        public string Description { get; set; }
    }
}