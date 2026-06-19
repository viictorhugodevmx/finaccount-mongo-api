namespace FinAccountMongoApi.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string AccountsCollectionName { get; set; } = string.Empty;
    public string MovementsCollectionName { get; set; } = string.Empty;
}
