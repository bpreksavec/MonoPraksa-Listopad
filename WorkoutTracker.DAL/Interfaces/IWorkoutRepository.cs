using WorkoutTracker.DAL.Models;

namespace WorkoutTracker.DAL.Interfaces;

public interface IWorkoutRepository
{
    /// <param name="userId">Only return this user's workouts; null returns everyone's.</param>
    Task<List<Workout>> GetAllAsync(Guid? userId);
    Task<Workout?> GetByIdAsync(Guid id);
    Task<Workout> CreateAsync(Workout workout);
    Task<bool> UpdateAsync(Guid id, Workout workout);
    Task<bool> DeleteAsync(Guid id);
}
