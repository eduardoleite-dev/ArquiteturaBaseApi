using ArquiteturaBaseApi.Models;

namespace ArquiteturaBaseApi.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken cancellationToken = default);
    Task UpdateLastLoginAsync(string id, DateTime lastLoginAt, CancellationToken cancellationToken = default);
}
