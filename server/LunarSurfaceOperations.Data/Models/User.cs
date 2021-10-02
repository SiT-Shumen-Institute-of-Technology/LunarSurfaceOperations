namespace LunarSurfaceOperations.Data.Models
{
    using MongoDB.Bson.Serialization.Attributes;

    public class User : BaseEntity
    {
        [BsonElement("u")]
        public string Username { get; set; }

        [BsonElement("p")]
        public string Password { get; set; }

        [BsonElement("s")]
        public string Salt { get; set; }
    }
}