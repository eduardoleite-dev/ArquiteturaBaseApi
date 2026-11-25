using System.ComponentModel.DataAnnotations;

namespace ArquiteturaBaseApi.DTOs;

public record LoginRequest(
    [property: Required(ErrorMessage = "O nome de usuário é obrigatório.")]
    string Username,

    [property: Required(ErrorMessage = "A senha é obrigatória.")]
    string Password
);
