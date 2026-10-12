using WebApplication3.Models;

namespace WebApplication3.Repositories;

public interface IWorkoutRepository
{
    Task<List<Workout>> GetAllAsync();
    Task<Workout?> GetByIdAsync(Guid id);
    Task<Workout> CreateAsync(Workout workout);
    Task<bool> UpdateAsync(Guid id, Workout workout);
    Task<bool> DeleteAsync(Guid id);
}