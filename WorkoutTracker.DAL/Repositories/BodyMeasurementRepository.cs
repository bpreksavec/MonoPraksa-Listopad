using Microsoft.EntityFrameworkCore;
using WorkoutTracker.DAL.Interfaces;
using WorkoutTracker.DAL.Models;

namespace WorkoutTracker.DAL.Repositories;

public class BodyMeasurementRepository : IBodyMeasurementRepository
{
    private readonly AppDbContext _context;

    public BodyMeasurementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BodyMeasurement>> GetAllByUserAsync(Guid userId)
    {
        return await _context.BodyMeasurements
            .AsNoTracking()
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.MeasuredAt)
            .ToListAsync();
    }

    public async Task<BodyMeasurement?> GetByIdAsync(Guid id)
    {
        return await _context.BodyMeasurements
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<BodyMeasurement> CreateAsync(BodyMeasurement measurement)
    {
        _context.BodyMeasurements.Add(measurement);
        await _context.SaveChangesAsync();

        return measurement;
    }

    public async Task<bool> UpdateAsync(Guid id, BodyMeasurement measurement)
    {
        var existingMeasurement = await _context.BodyMeasurements
            .FirstOrDefaultAsync(m => m.Id == id);

        if (existingMeasurement == null)
        {
            return false;
        }

        // UserId is intentionally not updatable
        existingMeasurement.HeightCm = measurement.HeightCm;
        existingMeasurement.WeightKg = measurement.WeightKg;
        existingMeasurement.MeasuredAt = measurement.MeasuredAt;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var measurement = await _context.BodyMeasurements
            .FirstOrDefaultAsync(m => m.Id == id);

        if (measurement == null)
        {
            return false;
        }

        _context.BodyMeasurements.Remove(measurement);

        await _context.SaveChangesAsync();

        return true;
    }
}
