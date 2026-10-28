using WorkoutTracker.DAL.Models;

namespace WorkoutTracker.DAL.Interfaces;

public interface IBodyMeasurementRepository
{
    Task<List<BodyMeasurement>> GetAllByUserAsync(Guid userId);
    Task<BodyMeasurement?> GetByIdAsync(Guid id);
    Task<BodyMeasurement> CreateAsync(BodyMeasurement measurement);
    Task<bool> UpdateAsync(Guid id, BodyMeasurement measurement);
    Task<bool> DeleteAsync(Guid id);
}
