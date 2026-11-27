using ArquiteturaBaseApi.DTOs;

namespace ArquiteturaBaseApi.Services.Interfaces;

public interface IUserService
{
    Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<UserResponse>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
