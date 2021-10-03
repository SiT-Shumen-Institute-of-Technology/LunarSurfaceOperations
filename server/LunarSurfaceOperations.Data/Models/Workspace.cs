namespace LunarSurfaceOperations.Data.Models
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Workspace : BaseEntity
    {
        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("d")]
        [BsonIgnoreIfDefault]
        public string Description { get; set; }

        [BsonElement("o")]
        public ObjectId OwnerId { get; [UsedImplicitly] set; }

        [BsonElement("m")]
        public List<ObjectId> Members { get; [UsedImplicitly] set; } = new();
    }
}