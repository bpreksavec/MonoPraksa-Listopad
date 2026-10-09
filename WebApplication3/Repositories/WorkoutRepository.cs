using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Repositories;

public class WorkoutRepository
{
    private readonly AppDbContext _context;

    public WorkoutRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Workout>> GetAll()
    {
        return await _context.Workouts.ToListAsync();
    }

    public async Task<Workout?> GetById(Guid id)
    {
        return await _context.Workouts
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Workout> Create(Workout workout)
    {
        _context.Workouts.Add(workout);

        await _context.SaveChangesAsync();

        return workout;
    }

    public async Task<bool> Update(Guid id, Workout workout)
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

    public async Task<bool> Delete(Guid id)
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