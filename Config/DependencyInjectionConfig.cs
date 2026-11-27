using ArquiteturaBaseApi.Repositories;
using ArquiteturaBaseApi.Repositories.Interfaces;
using ArquiteturaBaseApi.Services;
using ArquiteturaBaseApi.Services.Interfaces;

namespace ArquiteturaBaseApi.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDependencyInjectionConfig(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMongoTestService, MongoTestService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
