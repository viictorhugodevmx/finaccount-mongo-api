using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FinAccountMongoApi.Models;

public class MovementDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    [BsonElement("amount")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Amount { get; set; }

    [BsonElement("currency")]
    public string Currency { get; set; } = "MXN";

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("createdAtUtc")]
    public DateTime CreatedAtUtc { get; set; }
}
