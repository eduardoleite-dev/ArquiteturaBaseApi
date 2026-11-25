using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace ArquiteturaBaseApi.Config;

public static class RateLimiterConfig
{
    public static IServiceCollection AddRateLimiterConfig(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddFixedWindowLimiter(policyName: "LoginPolicy", fixedOptions =>
            {
                fixedOptions.PermitLimit = 5;
                fixedOptions.Window = TimeSpan.FromMinutes(1);
                fixedOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                fixedOptions.QueueLimit = 0;
            });
        });

        return services;
    }
}
