using System.ComponentModel.DataAnnotations;
using WorkoutTracker.DAL.Models;

namespace WorkoutTracker.Service.DTOs.BodyMeasurements;

public record BodyMeasurementRequest(
    [Range(30, 300)]
    double? HeightCm,

    [Range(1, 500)]
    double? WeightKg,

    // Defaults to now when omitted
    DateTime? MeasuredAt);

public record BodyMeasurementResponse(
    Guid Id,
    double? HeightCm,
    double? WeightKg,
    DateTime MeasuredAt)
{
    public static BodyMeasurementResponse FromEntity(BodyMeasurement measurement) => new(
        measurement.Id,
        measurement.HeightCm,
        measurement.WeightKg,
        measurement.MeasuredAt);
}
