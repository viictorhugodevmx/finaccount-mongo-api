namespace FinAccountMongoApi.Dtos;

public class AccountSummaryResponseDto
{
    public string AccountNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public string Currency { get; set; } = "MXN";
    public bool IsActive { get; set; }

    public int TotalMovements { get; set; }
    public int TotalDeposits { get; set; }
    public int TotalWithdrawals { get; set; }

    public decimal TotalDepositedAmount { get; set; }
    public decimal TotalWithdrawnAmount { get; set; }
}
