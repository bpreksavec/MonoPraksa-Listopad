using Microsoft.AspNetCore.Identity;
using WorkoutTracker.DAL.Interfaces;
using WorkoutTracker.DAL.Models;
using WorkoutTracker.Exceptions;
using WorkoutTracker.Service.DTOs.Auth;
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
        DateOfBirthValidator.Validate(request.DateOfBirth, nameof(request.DateOfBirth));

        var email = request.Email.Trim().ToLowerInvariant();

        if (await _users.ExistsAsync(email))
        {
            throw new ConflictException("Email is already taken.");
        }

        var userRole = await _users.GetRoleByNameAsync(Role.User)
            ?? throw new InvalidOperationException($"Role '{Role.User}' is missing from the database.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            CreatedAt = DateTime.UtcNow
        };

        user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = userRole.Id });
        user.PasswordHash = _hasher.HashPassword(user, request.Password);

        await _users.AddAsync(user);
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var user = await _users.GetByEmailAsync(request.Email.Trim().ToLowerInvariant());
        if (user == null)
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _hasher.HashPassword(user, request.Password);
            await _users.UpdateAsync(user);
        }

        return _tokenService.CreateToken(user);
    }
}
