namespace ArquiteturaBaseApi.DTOs;

public record UserResponse(
    string Id,
    string Name,
    string Email,
    DateTime? LastLoginAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int LevelAccess
);
