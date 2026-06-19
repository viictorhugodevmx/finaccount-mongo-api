namespace FinAccountMongoApi.Dtos;

public class MovementResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "MXN";
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
}
