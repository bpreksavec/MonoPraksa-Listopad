using WorkoutTracker.Models;

namespace WorkoutTracker.Services;

public interface IAuthService
{
    /// <exception cref="Exceptions.ConflictException">Username or email already taken.</exception>
    Task RegisterAsync(RegisterRequest request);

    /// <exception cref="Exceptions.UnauthorizedException">Wrong username or password.</exception>
    Task<string> LoginAsync(LoginRequest request);
}