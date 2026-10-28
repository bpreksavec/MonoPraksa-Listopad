using WorkoutTracker.DAL.Models;

namespace WorkoutTracker.DAL.Interfaces;

public interface IUserRepository
{
    /// <summary>Loads the user together with their roles.</summary>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>Loads the user together with their profile and roles, without tracking.</summary>
    Task<User?> GetByIdAsync(Guid id);

    Task<bool> ExistsAsync(string email);
    Task<Role?> GetRoleByNameAsync(string name);
    Task AddAsync(User user);
    Task UpdateAsync(User user);

    /// <summary>Updates the user's personal data and creates or updates their profile from user.Profile.</summary>
    Task<bool> UpdateProfileAsync(Guid id, User user);
}
