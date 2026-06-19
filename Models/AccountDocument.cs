using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FinAccountMongoApi.Models;

public class AccountDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [BsonElement("customerName")]
    public string CustomerName { get; set; } = string.Empty;

    [BsonElement("balance")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Balance { get; set; }

    [BsonElement("currency")]
    public string Currency { get; set; } = "MXN";

    [BsonElement("isActive")]
    public bool IsActive { get; set; }

    [BsonElement("createdAtUtc")]
    public DateTime CreatedAtUtc { get; set; }
}
