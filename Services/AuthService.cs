using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

    public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<LoginResponseDto> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameOrEmailAsync(username);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            throw new UnauthorizedException("Credenciais inválidas.");
        }

        await _userRepository.UpdateLastLoginAsync(user.Id, DateTime.UtcNow);

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
