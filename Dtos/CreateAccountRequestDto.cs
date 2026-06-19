namespace FinAccountMongoApi.Dtos;

public class CreateAccountRequestDto
{
    public string AccountNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
    public string Currency { get; set; } = "MXN";
}
