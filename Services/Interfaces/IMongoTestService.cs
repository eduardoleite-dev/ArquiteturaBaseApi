namespace ArquiteturaBaseApi.Services.Interfaces;

public interface IMongoTestService
{
    Task<string> TestConnectionAsync(CancellationToken cancellationToken = default);
}
