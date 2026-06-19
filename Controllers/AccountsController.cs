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
}
