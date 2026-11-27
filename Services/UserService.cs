using Microsoft.Extensions.Logging;
using ArquiteturaBaseApi.Common.Exceptions;
using ArquiteturaBaseApi.DTOs;
using ArquiteturaBaseApi.Models;
using ArquiteturaBaseApi.Repositories.Interfaces;
using ArquiteturaBaseApi.Services.Interfaces;

namespace ArquiteturaBaseApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByUsernameOrEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            _logger.LogWarning("Tentativa de criação de usuário com e-mail já existente: {Email}", request.Email);
            throw new ConflictException("Já existe um usuário cadastrado com este e-mail.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwordHash,
            LevelAccess = request.LevelAccess,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user, cancellationToken);

        _logger.LogInformation("Usuário criado com sucesso. Id: {UserId}, Email: {Email}", user.Id, user.Email);

        return MapToUserResponse(user);
    }

    public async Task<UserResponse> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("Usuário não encontrado com Id: {UserId}", id);
            throw new NotFoundException("Usuário não encontrado.");
        }

        return MapToUserResponse(user);
    }

    public async Task<PagedResult<UserResponse>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var pagedUsers = await _userRepository.GetPagedAsync(pageNumber, pageSize, cancellationToken);

        var userResponses = pagedUsers.Items
            .Select(MapToUserResponse)
            .ToList();

        return new PagedResult<UserResponse>(
            userResponses,
            pagedUsers.TotalItems,
            pagedUsers.PageNumber,
            pagedUsers.PageSize
        );
    }

    public async Task<UserResponse> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("Tentativa de atualizar usuário inexistente. Id: {UserId}", id);
            throw new NotFoundException("Usuário não encontrado.");
        }

        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existingUserWithEmail = await _userRepository.GetByUsernameOrEmailAsync(request.Email, cancellationToken);
            if (existingUserWithEmail != null && existingUserWithEmail.Id != id)
            {
                _logger.LogWarning("Tentativa de atualizar usuário {UserId} com e-mail já existente: {Email}", id, request.Email);
                throw new ConflictException("Já existe um usuário cadastrado com este e-mail.");
            }
        }

        user.Name = request.Name;
        user.Email = request.Email;
        user.LevelAccess = request.LevelAccess;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user, cancellationToken);

        _logger.LogInformation("Usuário atualizado com sucesso. Id: {UserId}", user.Id);

        return MapToUserResponse(user);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("Tentativa de deletar usuário inexistente. Id: {UserId}", id);
            throw new NotFoundException("Usuário não encontrado.");
        }

        await _userRepository.DeleteAsync(id, cancellationToken);

        _logger.LogInformation("Usuário removido com sucesso. Id: {UserId}", id);
    }

    private static UserResponse MapToUserResponse(User user)
    {
        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.LastLoginAt,
            user.CreatedAt,
            user.UpdatedAt,
            user.LevelAccess
        );
    }
}
