using FinAccountMongoApi.Dtos;
using FinAccountMongoApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FinAccountMongoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly AccountRepository _accountRepository;

    public AccountsController(AccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
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
}
