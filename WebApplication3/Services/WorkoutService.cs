using WebApplication3.Exceptions;
using WebApplication3.Models;
using WebApplication3.Repositories;

namespace WebApplication3.Services;

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _repository;
    private readonly WorkoutValidator _validator;

    public WorkoutService(IWorkoutRepository repository, WorkoutValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<List<Workout>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Workout> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Workout), id);
    }

    public async Task<Workout> AddAsync(WorkoutRequest request)
    {
        _validator.Validate(request);

        var workout = MapToEntity(request);
        workout.Id = Guid.NewGuid();

        return await _repository.CreateAsync(workout);
    }

    public async Task UpdateAsync(Guid id, WorkoutRequest request)
    {
        _validator.Validate(request);

        var updated = await _repository.UpdateAsync(id, MapToEntity(request));
        if (!updated)
        {
            throw new NotFoundException(nameof(Workout), id);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
        {
            throw new NotFoundException(nameof(Workout), id);
        }
    }

    private static Workout MapToEntity(WorkoutRequest request) => new()
    {
        Sport = request.Sport.Trim(),
        Distance = request.Distance,
        Duration = request.Duration,
        AvgHeartrate = request.AvgHeartrate,
        AthleteId = request.AthleteId
    };
}