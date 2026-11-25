using MongoDB.Bson;
using MongoDB.Driver;
using ArquiteturaBaseApi.Services.Interfaces;

namespace ArquiteturaBaseApi.Services;

public class MongoTestService : IMongoTestService
{
    private readonly IMongoDatabase _database;

    public MongoTestService(IMongoDatabase database)
    {
        _database = database;
    }

    public async Task<string> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var adminDb = _database.Client.GetDatabase("admin");
            var pingCommand = new BsonDocument("ping", 1);
            await adminDb.RunCommandAsync<BsonDocument>(pingCommand, cancellationToken: cancellationToken);
            return "Conexão com MongoDB estabelecida com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Falha na conexão com MongoDB: {ex.Message}";
        }
    }
}
