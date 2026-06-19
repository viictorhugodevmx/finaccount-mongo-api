namespace FinAccountMongoApi.Dtos;

public class CreateMovementRequestDto
{
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
}
