using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ArquiteturaBaseApi.Common.Exceptions;
using ArquiteturaBaseApi.DTOs;
using ArquiteturaBaseApi.Repositories.Interfaces;
using ArquiteturaBaseApi.Services.Interfaces;
using ArquiteturaBaseApi.Settings;

namespace ArquiteturaBaseApi.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IOptions<JwtSettings> jwtSettings,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public async Task<LoginResponseDto> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameOrEmailAsync(username, cancellationToken);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            _logger.LogWarning("Tentativa de login com credenciais inválidas para o usuário: {Username}", username);
            throw new UnauthorizedException("Credenciais inválidas.");
        }

        await _userRepository.UpdateLastLoginAsync(user.Id, DateTime.UtcNow, cancellationToken);

        _logger.LogInformation("Usuário {Username} (Id: {UserId}) autenticado com sucesso.", user.Name, user.Id);

        return GenerateJwtToken(user.Name, user.Id);
    }

    private LoginResponseDto GenerateJwtToken(string username, string userId)
    {
        var secretKey = _jwtSettings.SecretKey;
        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new InvalidOperationException("A chave secreta JWT não foi configurada em JwtSettings:SecretKey.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        
        var expiresAt = DateTime.UtcNow.AddHours(2);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] 
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userId)
            }),
            Expires = expiresAt,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new LoginResponseDto(tokenString, expiresAt, username, userId);
    }
}
