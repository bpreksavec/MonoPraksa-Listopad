using WebApplication3.Models;

namespace WebApplication3.Services;

public interface IWorkoutService
{
    Task<List<Workout>> GetAllAsync();
    Task<Workout?> GetByIdAsync(Guid id);
    Task<Workout> AddAsync(Workout workout);
    Task<bool> UpdateAsync(Guid id, Workout workout);
    Task<bool> DeleteAsync(Guid id);
}