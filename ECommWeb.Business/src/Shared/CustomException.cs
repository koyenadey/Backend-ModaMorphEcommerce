namespace ECommWeb.Business.src.Shared;

public class CustomException : Exception
{
    public int StatusCode { get; set; }

    public CustomException(int statusCode, string msg) : base(msg)
    {
        StatusCode = statusCode;
    }

    public static CustomException NotFoundException(string msg = "Not found")
    {
        return new CustomException(404, msg);
    }

    public static CustomException BadRequestException(string msg = "Bad request")
    {
        return new CustomException(400, msg);
    }

    public static CustomException UnauthorizedException(string msg = "Unauthorized")
    {
        return new CustomException(401, msg);
    }
}