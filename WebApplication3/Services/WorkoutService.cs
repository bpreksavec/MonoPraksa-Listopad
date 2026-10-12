using WebApplication3.Models;
using WebApplication3.Repositories;

namespace WebApplication3.Services;

public class WorkoutService : IWorkoutService
{
private readonly IWorkoutRepository _repository;
public WorkoutService(IWorkoutRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Workout>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Workout?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Workout> AddAsync(Workout workout)
    {
        return await _repository.CreateAsync(workout);
    }

    public async Task<bool> UpdateAsync(Guid id, Workout workout)
    {
        return await _repository.UpdateAsync(id, workout);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}
