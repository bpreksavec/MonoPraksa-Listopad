using Microsoft.AspNetCore.Identity;
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

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var username = request.Username.Trim().ToLowerInvariant();
        var email = request.Email.Trim().ToLowerInvariant();

        if (await _users.ExistsAsync(username, email))
        {
            return false;
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
        return true;
    }

    public async Task<string?> LoginAsync(LoginRequest request)
    {
        var user = await _users.GetByUsernameAsync(request.Username.Trim().ToLowerInvariant());
        if (user == null)
        {
            return null;
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
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