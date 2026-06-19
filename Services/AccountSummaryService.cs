using FinAccountMongoApi.Dtos;
using FinAccountMongoApi.Models;

namespace FinAccountMongoApi.Services;

public class AccountSummaryService
{
    public AccountSummaryResponseDto BuildSummary(
        AccountDocument account,
        List<MovementDocument> movements
    )
    {
        List<MovementDocument> deposits = movements
            .Where(movement => movement.Type.Equals("Deposit", StringComparison.OrdinalIgnoreCase))
            .ToList();

        List<MovementDocument> withdrawals = movements
            .Where(movement => movement.Type.Equals("Withdrawal", StringComparison.OrdinalIgnoreCase))
            .ToList();

        return new AccountSummaryResponseDto
        {
            AccountNumber = account.AccountNumber,
            CustomerName = account.CustomerName,
            CurrentBalance = account.Balance,
            Currency = account.Currency,
            IsActive = account.IsActive,
            TotalMovements = movements.Count,
            TotalDeposits = deposits.Count,
            TotalWithdrawals = withdrawals.Count,
            TotalDepositedAmount = deposits.Sum(movement => movement.Amount),
            TotalWithdrawnAmount = withdrawals.Sum(movement => movement.Amount)
        };
    }
}
