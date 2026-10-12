using WebApplication3.Models;

namespace WebApplication3.Services;

public interface IWorkoutService
{
    Task<List<Workout>> GetAllAsync();

    /// <exception cref="Exceptions.NotFoundException">Workout does not exist.</exception>
    Task<Workout> GetByIdAsync(Guid id);

    /// <exception cref="Exceptions.ValidationFailedException">Business rules failed.</exception>
    Task<Workout> AddAsync(WorkoutRequest request);

    /// <exception cref="Exceptions.NotFoundException">Workout does not exist.</exception>
    /// <exception cref="Exceptions.ValidationFailedException">Business rules failed.</exception>
    Task UpdateAsync(Guid id, WorkoutRequest request);

    /// <exception cref="Exceptions.NotFoundException">Workout does not exist.</exception>
    Task DeleteAsync(Guid id);
}