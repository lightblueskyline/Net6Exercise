using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ApiUseMongoDB.Models
{
    public class Book
    {
        [BsonId] // 文档主键
        [BsonRepresentation(BsonType.ObjectId)] // 允许参数以 string 类型传输给 ObjectId 类型
        public string? Id { get; set; }

        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string BookName { get; set; } = null!;

        public decimal Price { get; set; }

        public string Category { get; set; } = null!;

        public string Author { get; set; } = null!;
    }
}
