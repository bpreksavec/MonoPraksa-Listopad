using WorkoutTracker.Service.DTOs.Auth;

namespace WorkoutTracker.Service.Interfaces;

public interface IAuthService
{
    /// <exception cref="Exceptions.ConflictException">Email already taken.</exception>
    Task RegisterAsync(RegisterRequest request);

    /// <exception cref="Exceptions.UnauthorizedException">Wrong email or password.</exception>
    Task<string> LoginAsync(LoginRequest request);
}
