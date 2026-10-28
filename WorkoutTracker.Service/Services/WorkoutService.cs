using WorkoutTracker.DAL.Interfaces;
using WorkoutTracker.DAL.Models;
using WorkoutTracker.Exceptions;
using WorkoutTracker.Service.DTOs;
using WorkoutTracker.Service.DTOs.Workouts;
using WorkoutTracker.Service.Interfaces;

namespace WorkoutTracker.Service.Services;

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _repository;
    private readonly ISportRepository _sports;
    private readonly WorkoutValidator _validator;

    public WorkoutService(IWorkoutRepository repository, ISportRepository sports, WorkoutValidator validator)
    {
        _repository = repository;
        _sports = sports;
        _validator = validator;
    }

    public async Task<List<WorkoutResponse>> GetAllAsync(CurrentUser user)
    {
        var workouts = await _repository.GetAllAsync(user.IsAdmin ? null : user.Id);
        return workouts.Select(WorkoutResponse.FromEntity).ToList();
    }

    public async Task<WorkoutResponse> GetByIdAsync(Guid id, CurrentUser user)
    {
        return WorkoutResponse.FromEntity(await GetAccessibleAsync(id, user));
    }

    public async Task<WorkoutResponse> AddAsync(CurrentUser user, WorkoutRequest request)
    {
        await ValidateAsync(request);

        var workout = MapToEntity(Guid.NewGuid(), request);
        workout.UserId = user.Id;
        workout.CreatedAt = DateTime.UtcNow;

        return WorkoutResponse.FromEntity(await _repository.CreateAsync(workout));
    }

    public async Task UpdateAsync(Guid id, CurrentUser user, WorkoutRequest request)
    {
        await GetAccessibleAsync(id, user);
        await ValidateAsync(request);

        var updated = await _repository.UpdateAsync(id, MapToEntity(id, request));
        if (!updated)
        {
            throw new NotFoundException(nameof(Workout), id);
        }
    }

    public async Task DeleteAsync(Guid id, CurrentUser user)
    {
        await GetAccessibleAsync(id, user);

        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
        {
            throw new NotFoundException(nameof(Workout), id);
        }
    }

    // Someone else's workout is reported as not found, so ids of other users' workouts aren't revealed
    private async Task<Workout> GetAccessibleAsync(Guid id, CurrentUser user)
    {
        var workout = await _repository.GetByIdAsync(id);
        if (workout == null || (!user.IsAdmin && workout.UserId != user.Id))
        {
            throw new NotFoundException(nameof(Workout), id);
        }

        return workout;
    }

    private async Task ValidateAsync(WorkoutRequest request)
    {
        var sportIds = (request.Segments ?? [])
            .Select(s => s.DisciplineId)
            .Append(request.SportId);

        var sports = (await _sports.GetByIdsAsync(sportIds))
            .ToDictionary(s => s.Id);

        _validator.Validate(request, sports);
    }

    private static Workout MapToEntity(Guid id, WorkoutRequest request) => new()
    {
        Id = id,
        SportId = request.SportId,
        StartedAt = request.StartedAt.ToUniversalTime(),
        DurationSeconds = request.DurationSeconds,
        DistanceKm = request.DistanceKm,
        AvgHeartRate = request.AvgHeartRate,
        Calories = request.Calories,
        PerceivedEffort = request.PerceivedEffort,
        Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(),
        // List order becomes the 1-based SegmentOrder
        Segments = (request.Segments ?? [])
            .Select((s, index) => new WorkoutSegment
            {
                WorkoutId = id,
                SegmentOrder = index + 1,
                DisciplineId = s.DisciplineId,
                DurationSeconds = s.DurationSeconds,
                DistanceKm = s.DistanceKm
            })
            .ToList()
    };
}
