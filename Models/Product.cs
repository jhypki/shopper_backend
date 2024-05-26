using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShopperBackend.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; } = null!;

        [BsonElement("category")]
        public string? Category { get; set; } = null!;
        [BsonElement("description")]
        public string? Description { get; set; } = null!;
        [BsonElement("price")]

        public decimal Price { get; set; }


        [BsonElement("image_urls")]
        [JsonPropertyName("image_urls")]
        public List<string>? ImageUrls { get; set; } = new List<string>();
        [BsonElement("stock_quantity")]
        [JsonPropertyName("stock_quantity")]
        public int StockQuantity { get; set; }
    }
}
