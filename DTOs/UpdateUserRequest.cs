using System.ComponentModel.DataAnnotations;

namespace ArquiteturaBaseApi.DTOs;

public record UpdateUserRequest(
    [property: Required(ErrorMessage = "O nome é obrigatório.")]
    [property: StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    string Name,

    [property: Required(ErrorMessage = "O e-mail é obrigatório.")]
    [property: EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
    string Email,

    [property: Range(0, int.MaxValue, ErrorMessage = "O nível de acesso deve ser um valor válido.")]
    int LevelAccess
);
