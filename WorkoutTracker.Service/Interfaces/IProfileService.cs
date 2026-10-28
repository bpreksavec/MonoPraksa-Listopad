using WorkoutTracker.Service.DTOs.Profile;

namespace WorkoutTracker.Service.Interfaces;

public interface IProfileService
{
    /// <exception cref="Exceptions.NotFoundException">User does not exist.</exception>
    Task<ProfileResponse> GetAsync(Guid userId);

    /// <exception cref="Exceptions.NotFoundException">User does not exist.</exception>
    /// <exception cref="Exceptions.ValidationFailedException">Business rules failed.</exception>
    Task<ProfileResponse> UpdateAsync(Guid userId, ProfileRequest request);
}
