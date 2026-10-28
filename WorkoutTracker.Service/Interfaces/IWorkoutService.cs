using WorkoutTracker.Service.DTOs;
using WorkoutTracker.Service.DTOs.Workouts;

namespace WorkoutTracker.Service.Interfaces;

// Users only see and change their own workouts; admins can access everyone's
public interface IWorkoutService
{
    Task<List<WorkoutResponse>> GetAllAsync(CurrentUser user);

    /// <exception cref="Exceptions.NotFoundException">Workout does not exist or belongs to someone else.</exception>
    Task<WorkoutResponse> GetByIdAsync(Guid id, CurrentUser user);

    /// <exception cref="Exceptions.ValidationFailedException">Business rules failed.</exception>
    Task<WorkoutResponse> AddAsync(CurrentUser user, WorkoutRequest request);

    /// <exception cref="Exceptions.NotFoundException">Workout does not exist or belongs to someone else.</exception>
    /// <exception cref="Exceptions.ValidationFailedException">Business rules failed.</exception>
    Task UpdateAsync(Guid id, CurrentUser user, WorkoutRequest request);

    /// <exception cref="Exceptions.NotFoundException">Workout does not exist or belongs to someone else.</exception>
    Task DeleteAsync(Guid id, CurrentUser user);
}
