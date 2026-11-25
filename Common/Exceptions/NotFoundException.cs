namespace ArquiteturaBaseApi.Common.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message = "Recurso não encontrado.") 
        : base(message, StatusCodes.Status404NotFound)
    {
    }
}
