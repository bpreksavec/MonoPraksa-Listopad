using WebApplication3.Models;

namespace WebApplication3.Services;

public interface IAuthService
{
    /// <exception cref="Exceptions.ConflictException">Username or email already taken.</exception>
    Task RegisterAsync(RegisterRequest request);

    /// <exception cref="Exceptions.UnauthorizedException">Wrong username or password.</exception>
    Task<string> LoginAsync(LoginRequest request);
}