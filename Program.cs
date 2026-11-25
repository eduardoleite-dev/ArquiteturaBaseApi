using ArquiteturaBaseApi.Config;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCorsConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddMongoConfig(builder.Configuration);
builder.Services.AddDependencyInjectionConfig();
builder.Services.AddJwtConfig(builder.Configuration);
builder.Services.AddHealthCheckConfig();

var app = builder.Build();

app.UseMiddlewareConfig();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GaesdeApi v1");
    c.RoutePrefix = string.Empty; 
});

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");
app.Urls.Add("http://0.0.0.0:8080");
app.Run();
