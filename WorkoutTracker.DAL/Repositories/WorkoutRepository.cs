using Microsoft.EntityFrameworkCore;
using WorkoutTracker.DAL.Interfaces;
using WorkoutTracker.DAL.Models;

namespace WorkoutTracker.DAL.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly AppDbContext _context;

    public WorkoutRepository(AppDbContext context)
    {
        _context = context;
    }

    private IQueryable<Workout> WorkoutsWithDetails() => _context.Workouts
        .AsNoTracking()
        .Include(w => w.Sport)
        .Include(w => w.Segments.OrderBy(s => s.SegmentOrder))
            .ThenInclude(s => s.Discipline);

    public async Task<List<Workout>> GetAllAsync(Guid? userId)
    {
        return await WorkoutsWithDetails()
            .Where(w => userId == null || w.UserId == userId)
            .OrderByDescending(w => w.StartedAt)
            .ToListAsync();
    }

    public async Task<Workout?> GetByIdAsync(Guid id)
    {
        return await WorkoutsWithDetails()
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Workout> CreateAsync(Workout workout)
    {
        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();

        return await WorkoutsWithDetails()
            .FirstAsync(w => w.Id == workout.Id);
    }

    public async Task<bool> UpdateAsync(Guid id, Workout workout)
    {
        var existingWorkout = await _context.Workouts
            .Include(w => w.Segments)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (existingWorkout == null)
        {
            return false;
        }

        // UserId and CreatedAt are intentionally not updatable
        existingWorkout.SportId = workout.SportId;
        existingWorkout.StartedAt = workout.StartedAt;
        existingWorkout.DurationSeconds = workout.DurationSeconds;
        existingWorkout.DistanceKm = workout.DistanceKm;
        existingWorkout.AvgHeartRate = workout.AvgHeartRate;
        existingWorkout.Calories = workout.Calories;
        existingWorkout.PerceivedEffort = workout.PerceivedEffort;
        existingWorkout.Notes = workout.Notes;

        // Segments are keyed by (WorkoutId, SegmentOrder): update matching orders, add new, remove the rest
        foreach (var segment in workout.Segments)
        {
            var existingSegment = existingWorkout.Segments
                .FirstOrDefault(s => s.SegmentOrder == segment.SegmentOrder);

            if (existingSegment == null)
            {
                existingWorkout.Segments.Add(segment);
                continue;
            }

            existingSegment.DisciplineId = segment.DisciplineId;
            existingSegment.DurationSeconds = segment.DurationSeconds;
            existingSegment.DistanceKm = segment.DistanceKm;
        }

        var newOrders = workout.Segments.Select(s => s.SegmentOrder).ToHashSet();
        var removedSegments = existingWorkout.Segments
            .Where(s => !newOrders.Contains(s.SegmentOrder))
            .ToList();

        _context.WorkoutSegments.RemoveRange(removedSegments);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var workout = await _context.Workouts
            .FirstOrDefaultAsync(w => w.Id == id);

        if (workout == null)
        {
            return false;
        }

        _context.Workouts.Remove(workout);

        await _context.SaveChangesAsync();

        return true;
    }
}
