using FinAccountMongoApi.Dtos;
using FinAccountMongoApi.Models;

namespace FinAccountMongoApi.Services;

public class DashboardService
{
    public DashboardResponseDto BuildDashboard(
        List<AccountDocument> accounts,
        List<MovementDocument> movements
    )
    {
        List<MovementDocument> deposits = movements
            .Where(movement => movement.Type.Equals("Deposit", StringComparison.OrdinalIgnoreCase))
            .ToList();

        List<MovementDocument> withdrawals = movements
            .Where(movement => movement.Type.Equals("Withdrawal", StringComparison.OrdinalIgnoreCase))
            .ToList();

        return new DashboardResponseDto
        {
            TotalAccounts = accounts.Count,
            ActiveAccounts = accounts.Count(account => account.IsActive),
            InactiveAccounts = accounts.Count(account => !account.IsActive),
            TotalBalance = accounts.Sum(account => account.Balance),
            Currency = "MXN",
            TotalMovements = movements.Count,
            TotalDeposits = deposits.Count,
            TotalWithdrawals = withdrawals.Count,
            TotalDepositedAmount = deposits.Sum(movement => movement.Amount),
            TotalWithdrawnAmount = withdrawals.Sum(movement => movement.Amount),
            GeneratedAtUtc = DateTime.UtcNow
        };
    }
}
