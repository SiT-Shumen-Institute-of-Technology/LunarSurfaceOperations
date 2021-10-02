namespace LunarSurfaceOperations.Data.Models
{
    using MongoDB.Bson.Serialization.Attributes;

    public class User : BaseEntity
    {
        [BsonElement("e")]
        public string Email { get; set; }

        [BsonElement("u")]
        public string Username { get; set; }

        [BsonElement("p")]
        public string Password { get; set; }

        [BsonElement("s")]
        public string Salt { get; set; }
    }
}