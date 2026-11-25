namespace ArquiteturaBaseApi.Config;

public static class HealthCheckConfig
{
    public static IServiceCollection AddHealthCheckConfig(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<MongoHealthCheck>("mongodb");

        return services;
    }
}
