using ArquiteturaBaseApi.Middlewares;

namespace ArquiteturaBaseApi.Config;

public static class MiddlewareConfig
{
    public static IApplicationBuilder UseMiddlewareConfig(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
        return app;
    }
}
