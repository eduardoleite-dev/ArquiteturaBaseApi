using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ArquiteturaBaseApi.Config;

public class MongoHealthCheck : IHealthCheck
{
    private readonly IMongoDatabase _database;

    public MongoHealthCheck(IMongoDatabase database)
    {
        _database = database;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var pingCommand = new BsonDocument("ping", 1);
            await _database.RunCommandAsync<BsonDocument>(pingCommand, cancellationToken: cancellationToken);
            return HealthCheckResult.Healthy("Conexão com o MongoDB está operacional.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Falha na conexão com o MongoDB.", ex);
        }
    }
}
