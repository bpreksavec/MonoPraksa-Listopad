using WorkoutTracker.DAL.Interfaces;
using WorkoutTracker.DAL.Models;
using WorkoutTracker.Exceptions;
using WorkoutTracker.Service.DTOs.Profile;
using WorkoutTracker.Service.Interfaces;

namespace WorkoutTracker.Service.Services;

public class ProfileService : IProfileService
{
    private readonly IUserRepository _users;

    public ProfileService(IUserRepository users)
    {
        _users = users;
    }

    public async Task<ProfileResponse> GetAsync(Guid userId)
    {
        var user = await _users.GetByIdAsync(userId)
            ?? throw new NotFoundException(nameof(User), userId);

        return ProfileResponse.FromEntity(user);
    }

    public async Task<ProfileResponse> UpdateAsync(Guid userId, ProfileRequest request)
    {
        if (request.DateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ValidationFailedException(nameof(request.DateOfBirth), "Date of birth cannot be in the future.");
        }

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            Profile = new UserProfile
            {
                Bio = string.IsNullOrWhiteSpace(request.Bio) ? null : request.Bio.Trim(),
                ProfileImageUrl = string.IsNullOrWhiteSpace(request.ProfileImageUrl) ? null : request.ProfileImageUrl.Trim()
            }
        };

        var updated = await _users.UpdateProfileAsync(userId, user);
        if (!updated)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        return await GetAsync(userId);
    }
}
