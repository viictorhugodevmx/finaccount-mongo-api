using FinAccountMongoApi.Dtos;
using FinAccountMongoApi.Models;
using FinAccountMongoApi.Repositories;
using FinAccountMongoApi.Validators;

namespace FinAccountMongoApi.Services;

public class MovementApplicationService
{
    private readonly AccountRepository _accountRepository;
    private readonly MovementRepository _movementRepository;
    private readonly MovementValidator _movementValidator;

    public MovementApplicationService(
        AccountRepository accountRepository,
        MovementRepository movementRepository,
        MovementValidator movementValidator
    )
    {
        _accountRepository = accountRepository;
        _movementRepository = movementRepository;
        _movementValidator = movementValidator;
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

        OperationResult<bool> validationResult =
            _movementValidator.Validate(account, request);

        if (!validationResult.Success)
        {
            return OperationResult<MovementResponseDto>.Fail(
                validationResult.Message
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
