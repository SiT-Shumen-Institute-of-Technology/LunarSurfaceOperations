namespace LunarSurfaceOperations.Data.Models
{
    using MongoDB.Bson.Serialization.Attributes;

    [BsonDiscriminator("s")]
    public class MessageStringAttribute : IMessageAttribute
    {
        [BsonElement("an")]
        public string AttributeName { get; set; }

        [BsonElement("v")]
        public string Value { get; set; }
    }
}