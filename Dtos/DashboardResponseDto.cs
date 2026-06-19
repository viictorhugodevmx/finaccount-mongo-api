namespace FinAccountMongoApi.Dtos;

public class DashboardResponseDto
{
    public int TotalAccounts { get; set; }
    public int ActiveAccounts { get; set; }
    public int InactiveAccounts { get; set; }

    public decimal TotalBalance { get; set; }
    public string Currency { get; set; } = "MXN";

    public int TotalMovements { get; set; }
    public int TotalDeposits { get; set; }
    public int TotalWithdrawals { get; set; }

    public decimal TotalDepositedAmount { get; set; }
    public decimal TotalWithdrawnAmount { get; set; }

    public DateTime GeneratedAtUtc { get; set; }
}
