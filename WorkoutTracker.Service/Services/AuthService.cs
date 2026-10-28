using Microsoft.AspNetCore.Identity;
using WorkoutTracker.DAL.Interfaces;
using WorkoutTracker.Exceptions;
using WorkoutTracker.Models;
using WorkoutTracker.Service.Interfaces;

namespace WorkoutTracker.Service.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher<User> _hasher;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository users, IPasswordHasher<User> hasher, ITokenService tokenService)
    {
        _users = users;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var username = request.Username.Trim().ToLowerInvariant();
        var email = request.Email.Trim().ToLowerInvariant();

        if (await _users.ExistsAsync(username, email))
        {
            throw new ConflictException("Username or email is already taken.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            Role = Role.User
        };

        user.PasswordHash = _hasher.HashPassword(user, request.Password);

        await _users.AddAsync(user);
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var user = await _users.GetByUsernameAsync(request.Username.Trim().ToLowerInvariant());
        if (user == null)
        {
            throw new UnauthorizedException("Invalid username or password.");
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException("Invalid username or password.");
        }

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _hasher.HashPassword(user, request.Password);
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _users.UpdateAsync(user);

        return _tokenService.CreateToken(user);
    }
}