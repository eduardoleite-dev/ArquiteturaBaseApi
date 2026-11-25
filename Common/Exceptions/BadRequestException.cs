namespace ArquiteturaBaseApi.Common.Exceptions;

public class BadRequestException : AppException
{
    public BadRequestException(string message = "Requisição inválida.") 
        : base(message, StatusCodes.Status400BadRequest)
    {
    }
}
