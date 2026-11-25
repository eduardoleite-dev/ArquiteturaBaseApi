namespace ArquiteturaBaseApi.Common.Exceptions;

public class ConflictException : AppException
{
    public ConflictException(string message = "Conflito ao processar a requisição.") 
        : base(message, StatusCodes.Status409Conflict)
    {
    }
}
