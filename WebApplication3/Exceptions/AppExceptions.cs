namespace WebApplication3.Exceptions;
public abstract class AppException : Exception
{
    public int StatusCode { get; }

    protected AppException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}

public sealed class NotFoundException : AppException
{
    public NotFoundException(string resource, object key)
        : base($"{resource} with id '{key}' was not found.", StatusCodes.Status404NotFound)
    {
    }
}

public sealed class ConflictException : AppException
{
    public ConflictException(string message)
        : base(message, StatusCodes.Status409Conflict)
    {
    }
}

public sealed class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Invalid credentials.")
        : base(message, StatusCodes.Status401Unauthorized)
    {
    }
}

public sealed class ForbiddenException : AppException
{
    public ForbiddenException(string message = "You do not have access to this resource.")
        : base(message, StatusCodes.Status403Forbidden)
    {
    }
}

public sealed class ValidationFailedException : AppException
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationFailedException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.", StatusCodes.Status400BadRequest)
    {
        Errors = errors;
    }

    public ValidationFailedException(string field, string error)
        : this(new Dictionary<string, string[]> { [field] = [error] })
    {
    }
}