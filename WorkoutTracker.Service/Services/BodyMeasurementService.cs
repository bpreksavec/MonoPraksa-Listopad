using WorkoutTracker.DAL.Interfaces;
using WorkoutTracker.DAL.Models;
using WorkoutTracker.Exceptions;
using WorkoutTracker.Service.DTOs.BodyMeasurements;
using WorkoutTracker.Service.Interfaces;

namespace WorkoutTracker.Service.Services;

public class BodyMeasurementService : IBodyMeasurementService
{
    private readonly IBodyMeasurementRepository _repository;

    public BodyMeasurementService(IBodyMeasurementRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BodyMeasurementResponse>> GetAllAsync(Guid userId)
    {
        var measurements = await _repository.GetAllByUserAsync(userId);
        return measurements.Select(BodyMeasurementResponse.FromEntity).ToList();
    }

    public async Task<BodyMeasurementResponse> GetByIdAsync(Guid id, Guid userId)
    {
        return BodyMeasurementResponse.FromEntity(await GetOwnedAsync(id, userId));
    }

    public async Task<BodyMeasurementResponse> AddAsync(Guid userId, BodyMeasurementRequest request)
    {
        Validate(request);

        var measurement = MapToEntity(request);
        measurement.Id = Guid.NewGuid();
        measurement.UserId = userId;

        return BodyMeasurementResponse.FromEntity(await _repository.CreateAsync(measurement));
    }

    public async Task UpdateAsync(Guid id, Guid userId, BodyMeasurementRequest request)
    {
        await GetOwnedAsync(id, userId);
        Validate(request);

        var updated = await _repository.UpdateAsync(id, MapToEntity(request));
        if (!updated)
        {
            throw new NotFoundException(nameof(BodyMeasurement), id);
        }
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        await GetOwnedAsync(id, userId);

        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
        {
            throw new NotFoundException(nameof(BodyMeasurement), id);
        }
    }

    // Someone else's measurement is reported as not found, so its existence isn't revealed
    private async Task<BodyMeasurement> GetOwnedAsync(Guid id, Guid userId)
    {
        var measurement = await _repository.GetByIdAsync(id);
        if (measurement == null || measurement.UserId != userId)
        {
            throw new NotFoundException(nameof(BodyMeasurement), id);
        }

        return measurement;
    }

    private static void Validate(BodyMeasurementRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (request.HeightCm is null && request.WeightKg is null)
        {
            errors[nameof(request.WeightKg)] = ["Either height or weight must be provided."];
        }

        if (request.MeasuredAt?.ToUniversalTime() > DateTime.UtcNow.AddMinutes(5))
        {
            errors[nameof(request.MeasuredAt)] = ["A measurement cannot be in the future."];
        }

        if (errors.Count > 0)
        {
            throw new ValidationFailedException(errors);
        }
    }

    private static BodyMeasurement MapToEntity(BodyMeasurementRequest request) => new()
    {
        HeightCm = request.HeightCm,
        WeightKg = request.WeightKg,
        MeasuredAt = request.MeasuredAt?.ToUniversalTime() ?? DateTime.UtcNow
    };
}
