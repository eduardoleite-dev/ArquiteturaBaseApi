using ArquiteturaBaseApi.DTOs;
using ArquiteturaBaseApi.Models;

namespace ArquiteturaBaseApi.Repositories.Interfaces;

public interface IUserRepository
{
    Task CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken cancellationToken = default);
    Task UpdateLastLoginAsync(string id, DateTime lastLoginAt, CancellationToken cancellationToken = default);
    Task<PagedResult<User>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
