using WebApplication3.Models;
using WebApplication3.Repositories;

namespace WebApplication3.Services;

public class WorkoutService
{
    private readonly WorkoutRepository _repository;
public WorkoutService(WorkoutRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Workout>> GetAllAsync()
    {
        return await _repository.GetAll();
    }

    public async Task<Workout?> GetByIdAsync(Guid id)
    {
        return await _repository.GetById(id);
    }

    public async Task<Workout> AddAsync(Workout workout)
    {
        return await _repository.Create(workout);
    }

    public async Task<bool> UpdateAsync(Guid id, Workout workout)
    {
        return await _repository.Update(id, workout);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.Delete(id);
    }
}
