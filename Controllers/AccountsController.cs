using FinAccountMongoApi.Dtos;
using FinAccountMongoApi.Helpers;
using FinAccountMongoApi.Models;
using FinAccountMongoApi.Repositories;
using FinAccountMongoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinAccountMongoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly AccountRepository _accountRepository;
    private readonly MovementRepository _movementRepository;
    private readonly MovementApplicationService _movementApplicationService;
    private readonly AccountSummaryService _accountSummaryService;

    public AccountsController(
        AccountRepository accountRepository,
        MovementRepository movementRepository,
        MovementApplicationService movementApplicationService,
        AccountSummaryService accountSummaryService
    )
    {
        _accountRepository = accountRepository;
        _movementRepository = movementRepository;
        _movementApplicationService = movementApplicationService;
        _accountSummaryService = accountSummaryService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<AccountResponseDto>>>> GetAccounts()
    {
        List<AccountResponseDto> accounts =
            await _accountRepository.GetAccountsAsync();

        return Ok(ApiResponse<List<AccountResponseDto>>.Ok(
            accounts,
            "Accounts retrieved successfully."
        ));
    }

    [HttpGet("{accountNumber}")]
    public async Task<ActionResult<ApiResponse<AccountResponseDto>>> GetAccountByNumber(
        string accountNumber
    )
    {
        AccountResponseDto? account =
            await _accountRepository.GetAccountByNumberAsync(accountNumber);

        if (account is null)
        {
            return NotFound(ApiResponse<AccountResponseDto>.Fail(
                $"Account {accountNumber} was not found."
            ));
        }

        return Ok(ApiResponse<AccountResponseDto>.Ok(
            account,
            "Account retrieved successfully."
        ));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<AccountResponseDto>>> CreateAccount(
        [FromBody] CreateAccountRequestDto request
    )
    {
        if (string.IsNullOrWhiteSpace(request.AccountNumber))
            return BadRequest(ApiResponse<AccountResponseDto>.Fail("Account number is required."));

        if (string.IsNullOrWhiteSpace(request.CustomerName))
            return BadRequest(ApiResponse<AccountResponseDto>.Fail("Customer name is required."));

        if (request.InitialBalance < 0)
            return BadRequest(ApiResponse<AccountResponseDto>.Fail("Initial balance cannot be negative."));

        if (string.IsNullOrWhiteSpace(request.Currency))
            return BadRequest(ApiResponse<AccountResponseDto>.Fail("Currency is required."));

        AccountResponseDto? createdAccount =
            await _accountRepository.CreateAccountAsync(request);

        if (createdAccount is null)
        {
            return Conflict(ApiResponse<AccountResponseDto>.Fail(
                $"Account {request.AccountNumber} already exists."
            ));
        }

        return CreatedAtAction(
            nameof(GetAccountByNumber),
            new { accountNumber = createdAccount.AccountNumber },
            ApiResponse<AccountResponseDto>.Ok(
                createdAccount,
                "Account created successfully."
            )
        );
    }

    [HttpPatch("{accountNumber}/status")]
    public async Task<ActionResult<ApiResponse<AccountResponseDto>>> UpdateAccountStatus(
        string accountNumber,
        [FromBody] UpdateAccountStatusRequestDto request
    )
    {
        AccountResponseDto? updatedAccount =
            await _accountRepository.UpdateAccountStatusAsync(accountNumber, request);

        if (updatedAccount is null)
        {
            return NotFound(ApiResponse<AccountResponseDto>.Fail(
                $"Account {accountNumber} was not found."
            ));
        }

        return Ok(ApiResponse<AccountResponseDto>.Ok(
            updatedAccount,
            "Account status updated successfully."
        ));
    }

    [HttpGet("{accountNumber}/movements")]
    public async Task<ActionResult<ApiResponse<List<MovementResponseDto>>>> GetMovements(
        string accountNumber
    )
    {
        AccountResponseDto? account =
            await _accountRepository.GetAccountByNumberAsync(accountNumber);

        if (account is null)
        {
            return NotFound(ApiResponse<List<MovementResponseDto>>.Fail(
                $"Account {accountNumber} was not found."
            ));
        }

        List<MovementResponseDto> movements =
            await _movementRepository.GetMovementsByAccountNumberAsync(accountNumber);

        return Ok(ApiResponse<List<MovementResponseDto>>.Ok(
            movements,
            "Movements retrieved successfully."
        ));
    }

    [HttpPost("{accountNumber}/movements")]
    public async Task<ActionResult<ApiResponse<MovementResponseDto>>> CreateMovement(
        string accountNumber,
        [FromBody] CreateMovementRequestDto request
    )
    {
        OperationResult<MovementResponseDto> result =
            await _movementApplicationService.CreateMovementAsync(accountNumber, request);

        if (!result.Success)
        {
            return ApiResponseHelper.FromFailedOperation(this, result);
        }

        return Created(
            $"/api/accounts/{accountNumber}/movements/{result.Data!.Id}",
            ApiResponse<MovementResponseDto>.Ok(
                result.Data,
                result.Message
            )
        );
    }

    [HttpGet("{accountNumber}/summary")]
    public async Task<ActionResult<ApiResponse<AccountSummaryResponseDto>>> GetSummary(
        string accountNumber
    )
    {
        AccountDocument? account =
            await _accountRepository.GetRawAccountByNumberAsync(accountNumber);

        if (account is null)
        {
            return NotFound(ApiResponse<AccountSummaryResponseDto>.Fail(
                $"Account {accountNumber} was not found."
            ));
        }

        List<MovementDocument> movements =
            await _movementRepository.GetRawMovementsByAccountNumberAsync(accountNumber);

        AccountSummaryResponseDto summary =
            _accountSummaryService.BuildSummary(account, movements);

        return Ok(ApiResponse<AccountSummaryResponseDto>.Ok(
            summary,
            "Account summary retrieved successfully."
        ));
    }
}
