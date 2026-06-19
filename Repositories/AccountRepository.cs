using FinAccountMongoApi.Dtos;
using FinAccountMongoApi.Models;
using FinAccountMongoApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FinAccountMongoApi.Repositories;

public class AccountRepository
{
    private readonly IMongoCollection<AccountDocument> _accountsCollection;

    public AccountRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        MongoClient mongoClient = new MongoClient(
            mongoDbSettings.Value.ConnectionString
        );

        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(
            mongoDbSettings.Value.DatabaseName
        );

        _accountsCollection = mongoDatabase.GetCollection<AccountDocument>(
            mongoDbSettings.Value.AccountsCollectionName
        );
    }

    public async Task<List<AccountResponseDto>> GetAccountsAsync()
    {
        List<AccountDocument> accounts = await _accountsCollection
            .Find(_ => true)
            .ToListAsync();

        return accounts.Select(MapToResponseDto).ToList();
    }

    public async Task<AccountResponseDto?> GetAccountByNumberAsync(string accountNumber)
    {
        AccountDocument? account = await _accountsCollection
            .Find(account => account.AccountNumber == accountNumber.ToUpper())
            .FirstOrDefaultAsync();

        if (account is null)
        {
            return null;
        }

        return MapToResponseDto(account);
    }

    public async Task<AccountResponseDto?> CreateAccountAsync(CreateAccountRequestDto request)
    {
        string normalizedAccountNumber = request.AccountNumber.Trim().ToUpper();

        bool alreadyExists = await _accountsCollection
            .Find(account => account.AccountNumber == normalizedAccountNumber)
            .AnyAsync();

        if (alreadyExists)
        {
            return null;
        }

        AccountDocument account = new AccountDocument
        {
            AccountNumber = normalizedAccountNumber,
            CustomerName = request.CustomerName.Trim(),
            Balance = request.InitialBalance,
            Currency = request.Currency.Trim().ToUpper(),
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _accountsCollection.InsertOneAsync(account);

        return MapToResponseDto(account);
    }

    public async Task<AccountResponseDto?> UpdateAccountStatusAsync(
        string accountNumber,
        UpdateAccountStatusRequestDto request
    )
    {
        string normalizedAccountNumber = accountNumber.Trim().ToUpper();

        UpdateDefinition<AccountDocument> update = Builders<AccountDocument>
            .Update
            .Set(account => account.IsActive, request.IsActive);

        AccountDocument? updatedAccount = await _accountsCollection
            .FindOneAndUpdateAsync(
                account => account.AccountNumber == normalizedAccountNumber,
                update,
                new FindOneAndUpdateOptions<AccountDocument>
                {
                    ReturnDocument = ReturnDocument.After
                }
            );

        if (updatedAccount is null)
        {
            return null;
        }

        return MapToResponseDto(updatedAccount);
    }

    private static AccountResponseDto MapToResponseDto(AccountDocument account)
    {
        return new AccountResponseDto
        {
            Id = account.Id ?? string.Empty,
            AccountNumber = account.AccountNumber,
            CustomerName = account.CustomerName,
            Balance = account.Balance,
            Currency = account.Currency,
            IsActive = account.IsActive,
            CreatedAtUtc = account.CreatedAtUtc
        };
    }
}
