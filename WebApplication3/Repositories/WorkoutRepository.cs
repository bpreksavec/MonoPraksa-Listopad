using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly AppDbContext _context;

    public WorkoutRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Workout>> GetAllAsync()
    {
        return await _context.Workouts.AsNoTracking().ToListAsync();
    }

    public async Task<Workout?> GetByIdAsync(Guid id)
    {
        return await _context.Workouts.FindAsync(id);
    }

    public async Task<Workout> CreateAsync(Workout workout)
    {
        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();

        return workout;
    }

    public async Task<bool> UpdateAsync(Guid id, Workout workout)
    {
        var existing = await _context.Workouts.FindAsync(id);
        if (existing == null)
        {
            return false;
        }

        existing.Sport = workout.Sport;
        existing.Distance = workout.Distance;
        existing.Duration = workout.Duration;
        existing.AvgHeartrate = workout.AvgHeartrate;
        existing.AthleteId = workout.AthleteId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var workout = await _context.Workouts.FindAsync(id);
        if (workout == null)
        {
            return false;
        }

        _context.Workouts.Remove(workout);
        await _context.SaveChangesAsync();
        return true;
    }
}
