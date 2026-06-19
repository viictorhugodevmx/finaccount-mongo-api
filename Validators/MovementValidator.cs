using FinAccountMongoApi.Dtos;
using FinAccountMongoApi.Models;

namespace FinAccountMongoApi.Validators;

public class MovementValidator
{
    public OperationResult<bool> Validate(
        AccountDocument account,
        CreateMovementRequestDto request
    )
    {
        if (!account.IsActive)
        {
            return OperationResult<bool>.Fail(
                $"Account {account.AccountNumber} is not active."
            );
        }

        if (request.Amount <= 0)
        {
            return OperationResult<bool>.Fail(
                "Movement amount must be greater than zero."
            );
        }

        bool isDeposit = request.Type.Equals("Deposit", StringComparison.OrdinalIgnoreCase);
        bool isWithdrawal = request.Type.Equals("Withdrawal", StringComparison.OrdinalIgnoreCase);

        if (!isDeposit && !isWithdrawal)
        {
            return OperationResult<bool>.Fail(
                "Movement type must be Deposit or Withdrawal."
            );
        }

        if (isWithdrawal && request.Amount > account.Balance)
        {
            return OperationResult<bool>.Fail(
                "Insufficient funds."
            );
        }

        return OperationResult<bool>.Ok(
            true,
            "Movement validation approved."
        );
    }
}
