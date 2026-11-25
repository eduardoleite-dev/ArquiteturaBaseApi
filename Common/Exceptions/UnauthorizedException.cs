namespace ArquiteturaBaseApi.Common.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Acesso não autorizado.") 
        : base(message, StatusCodes.Status401Unauthorized)
    {
    }
}
