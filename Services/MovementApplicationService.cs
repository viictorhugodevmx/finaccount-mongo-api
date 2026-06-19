using FinAccountMongoApi.Dtos;
using FinAccountMongoApi.Models;
using FinAccountMongoApi.Repositories;

namespace FinAccountMongoApi.Services;

public class MovementApplicationService
{
    private readonly AccountRepository _accountRepository;
    private readonly MovementRepository _movementRepository;

    public MovementApplicationService(
        AccountRepository accountRepository,
        MovementRepository movementRepository
    )
    {
        _accountRepository = accountRepository;
        _movementRepository = movementRepository;
    }

    public async Task<OperationResult<MovementResponseDto>> CreateMovementAsync(
        string accountNumber,
        CreateMovementRequestDto request
    )
    {
        AccountDocument? account =
            await _accountRepository.GetRawAccountByNumberAsync(accountNumber);

        if (account is null)
        {
            return OperationResult<MovementResponseDto>.Fail(
                $"Account {accountNumber} was not found."
            );
        }

        if (!account.IsActive)
        {
            return OperationResult<MovementResponseDto>.Fail(
                $"Account {accountNumber} is not active."
            );
        }

        if (request.Amount <= 0)
        {
            return OperationResult<MovementResponseDto>.Fail(
                "Movement amount must be greater than zero."
            );
        }

        bool isDeposit = request.Type.Equals("Deposit", StringComparison.OrdinalIgnoreCase);
        bool isWithdrawal = request.Type.Equals("Withdrawal", StringComparison.OrdinalIgnoreCase);

        if (!isDeposit && !isWithdrawal)
        {
            return OperationResult<MovementResponseDto>.Fail(
                "Movement type must be Deposit or Withdrawal."
            );
        }

        if (isWithdrawal && request.Amount > account.Balance)
        {
            return OperationResult<MovementResponseDto>.Fail(
                "Insufficient funds."
            );
        }

        MovementResponseDto movement =
            await _movementRepository.CreateMovementAsync(account, request);

        await _accountRepository.UpdateBalanceAsync(
            account.AccountNumber,
            movement.Type,
            movement.Amount
        );

        return OperationResult<MovementResponseDto>.Ok(
            movement,
            "Movement created successfully."
        );
    }
}
