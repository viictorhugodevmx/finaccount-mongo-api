using FinAccountMongoApi.Dtos;
using FinAccountMongoApi.Models;
using FinAccountMongoApi.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FinAccountMongoApi.Repositories;

public class MovementRepository
{
    private readonly IMongoCollection<MovementDocument> _movementsCollection;

    public MovementRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        MongoClient mongoClient = new MongoClient(
            mongoDbSettings.Value.ConnectionString
        );

        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(
            mongoDbSettings.Value.DatabaseName
        );

        _movementsCollection = mongoDatabase.GetCollection<MovementDocument>(
            mongoDbSettings.Value.MovementsCollectionName
        );
    }

    public async Task<List<MovementResponseDto>> GetMovementsByAccountNumberAsync(
        string accountNumber
    )
    {
        string normalizedAccountNumber = accountNumber.Trim().ToUpper();

        List<MovementDocument> movements = await _movementsCollection
            .Find(movement => movement.AccountNumber == normalizedAccountNumber)
            .SortByDescending(movement => movement.CreatedAtUtc)
            .ToListAsync();

        return movements.Select(MapToResponseDto).ToList();
    }

    public async Task<MovementResponseDto> CreateMovementAsync(
        AccountDocument account,
        CreateMovementRequestDto request
    )
    {
        MovementDocument movement = new MovementDocument
        {
            AccountNumber = account.AccountNumber,
            Type = NormalizeMovementType(request.Type),
            Amount = request.Amount,
            Currency = account.Currency,
            Description = request.Description.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };

        await _movementsCollection.InsertOneAsync(movement);

        return MapToResponseDto(movement);
    }

    private static string NormalizeMovementType(string type)
    {
        if (type.Equals("Deposit", StringComparison.OrdinalIgnoreCase))
        {
            return "Deposit";
        }

        if (type.Equals("Withdrawal", StringComparison.OrdinalIgnoreCase))
        {
            return "Withdrawal";
        }

        return type;
    }

    private static MovementResponseDto MapToResponseDto(MovementDocument movement)
    {
        return new MovementResponseDto
        {
            Id = movement.Id ?? string.Empty,
            AccountNumber = movement.AccountNumber,
            Type = movement.Type,
            Amount = movement.Amount,
            Currency = movement.Currency,
            Description = movement.Description,
            CreatedAtUtc = movement.CreatedAtUtc
        };
    }
}
