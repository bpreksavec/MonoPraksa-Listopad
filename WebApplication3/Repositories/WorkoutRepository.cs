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
        return await _context.Workouts
            .ToListAsync();
    }

    public async Task<Workout?> GetByIdAsync(Guid id)
    {
        return await _context.Workouts
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Workout> CreateAsync(Workout workout)
    {
        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();

        return await _context.Workouts
            .FirstAsync(w => w.Id == workout.Id);
    }

    public async Task<bool> UpdateAsync(Guid id, Workout workout)
    {
        var existingWorkout = await _context.Workouts
            .FirstOrDefaultAsync(w => w.Id == id);

        if (existingWorkout == null)
        {
            return false;
        }

        existingWorkout.Sport = workout.Sport;
        existingWorkout.Distance = workout.Distance;
        existingWorkout.Duration = workout.Duration;
        existingWorkout.AvgHeartrate = workout.AvgHeartrate;
        existingWorkout.AthleteId = workout.AthleteId;

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