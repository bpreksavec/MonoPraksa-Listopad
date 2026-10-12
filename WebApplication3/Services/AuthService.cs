using Microsoft.AspNetCore.Identity;
using WebApplication3.Exceptions;
using WebApplication3.Models;
using WebApplication3.Repositories;

namespace WebApplication3.Services;

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

    private const string InvalidCredentialsMessage = "Invalid username or password.";

    // Usernames and emails are stored lowercase, so every lookup must normalize the same way
    private static string Normalize(string value) => value.Trim().ToLowerInvariant();

    public async Task RegisterAsync(RegisterRequest request)
    {
        var username = Normalize(request.Username);
        var email = Normalize(request.Email);

        if (await _users.ExistsAsync(username, email))
        {
            throw new ConflictException("Username or email is already taken.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            Role = Roles.User
        };

        user.PasswordHash = _hasher.HashPassword(user, request.Password);

        await _users.AddAsync(user);
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var user = await _users.GetByUsernameAsync(Normalize(request.Username));
        if (user == null)
        {
            throw new UnauthorizedException(InvalidCredentialsMessage);
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException(InvalidCredentialsMessage);
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