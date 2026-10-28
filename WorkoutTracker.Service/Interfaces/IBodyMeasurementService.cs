using WorkoutTracker.Service.DTOs.BodyMeasurements;

namespace WorkoutTracker.Service.Interfaces;

// All operations are scoped to the given user's own measurements
public interface IBodyMeasurementService
{
    Task<List<BodyMeasurementResponse>> GetAllAsync(Guid userId);

    /// <exception cref="Exceptions.NotFoundException">Measurement does not exist or belongs to someone else.</exception>
    Task<BodyMeasurementResponse> GetByIdAsync(Guid id, Guid userId);

    /// <exception cref="Exceptions.ValidationFailedException">Business rules failed.</exception>
    Task<BodyMeasurementResponse> AddAsync(Guid userId, BodyMeasurementRequest request);

    /// <exception cref="Exceptions.NotFoundException">Measurement does not exist or belongs to someone else.</exception>
    /// <exception cref="Exceptions.ValidationFailedException">Business rules failed.</exception>
    Task UpdateAsync(Guid id, Guid userId, BodyMeasurementRequest request);

    /// <exception cref="Exceptions.NotFoundException">Measurement does not exist or belongs to someone else.</exception>
    Task DeleteAsync(Guid id, Guid userId);
}
