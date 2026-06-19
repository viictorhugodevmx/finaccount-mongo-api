namespace FinAccountMongoApi.Dtos;

public class AccountResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "MXN";
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
