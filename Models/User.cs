using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ShopperBackend.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("username")]
        [JsonPropertyName("username")]
        public string? Username { get; set; } = null!;

        [BsonElement("password_hash")]
        [JsonPropertyName("password_hash")]
        public string PasswordHash { get; set; } = null!;

        [BsonElement("email")]
        [JsonPropertyName("email")]
        public string? Email { get; set; } = null!;

        [BsonElement("first_name")]
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; } = null!;

        [BsonElement("last_name")]
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; } = null!;

        [BsonElement("address")]
        [JsonPropertyName("address")]
        public string? Address { get; set; }
    }
}
