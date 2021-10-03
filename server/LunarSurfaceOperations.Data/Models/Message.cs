namespace LunarSurfaceOperations.Data.Models
{
    using System;
    using System.Collections.Generic;
    using LunarSurfaceOperations.Data.Contracts;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Message : BaseEntity, IWorkspaceDependentEntity
    {
        [BsonElement("wid")]
        public ObjectId WorkspaceId { get; set; }

        [BsonElement("t")]
        public string Text { get; set; }
        
        [BsonElement("aid")]
        public ObjectId AuthorId { get; set; }
        
        [BsonElement("d")]
        public DateTime Timestamp { get; set; }

        [BsonElement("a")]
        public List<IMessageAttribute> Attributes { get; set; } = new List<IMessageAttribute>();
    }
}