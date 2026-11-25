namespace ArquiteturaBaseApi.Common.Exceptions;

public abstract class AppException : Exception
{
    public int StatusCode { get; }

    protected AppException(string message, int statusCode = StatusCodes.Status400BadRequest) 
        : base(message)
    {
        StatusCode = statusCode;
    }
}
