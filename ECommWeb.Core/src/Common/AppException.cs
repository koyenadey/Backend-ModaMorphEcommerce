using System.Net;

namespace ECommWeb.Core.src.Common;

public class AppException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }

    // Exception Factory
    public static AppException Create(HttpStatusCode statusCode, string message)
    {
        return new AppException
        {
            StatusCode = statusCode,
            Message = message
        };
    }
}


public class ValidationException : AppException
{
    public ValidationException(string message) : base()
    {
        StatusCode = HttpStatusCode.BadRequest;
        Message = message;
    }
}

public class OperationFailedException : AppException
{
    public OperationFailedException(string message) : base()
    {
        StatusCode = HttpStatusCode.ExpectationFailed;
        Message = message;
    }
}

public class ResourceNotFoundException : AppException
{
    public ResourceNotFoundException(string message) : base()
    {
        StatusCode = HttpStatusCode.NotFound;
        Message = message;
    }
}

public class PermissionDeniedException : AppException
{
    public PermissionDeniedException(string message) : base()
    {
        StatusCode = HttpStatusCode.Forbidden;
        Message = message;
    }
}
