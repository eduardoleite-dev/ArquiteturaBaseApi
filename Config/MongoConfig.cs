using ArquiteturaBaseApi.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ArquiteturaBaseApi.Config;

public static class MongoConfig
{
    public static IServiceCollection AddMongoConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            var connectionString = configuration["ConnectionStrings__MongoDB"] 
                                   ?? configuration["ConnectionStrings:MongoDB"] 
                                   ?? settings.ConnectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("String de conexão do MongoDB não encontrada.");
            }

            return new MongoClient(connectionString);
        });

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            var mongoClient = sp.GetRequiredService<IMongoClient>();

            var databaseName = !string.IsNullOrWhiteSpace(settings.DatabaseName)
                ? settings.DatabaseName
                : MongoUrl.Create(configuration["ConnectionStrings__MongoDB"] 
                                  ?? configuration["ConnectionStrings:MongoDB"] 
                                  ?? settings.ConnectionString).DatabaseName;

            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new InvalidOperationException("O nome do banco de dados MongoDB não foi configurado em MongoDbSettings:DatabaseName.");
            }

            return mongoClient.GetDatabase(databaseName);
        });

        return services;
    }
}
