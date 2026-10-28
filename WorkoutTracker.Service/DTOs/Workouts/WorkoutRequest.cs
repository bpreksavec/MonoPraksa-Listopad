using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Service.DTOs.Workouts;

public record WorkoutRequest(
    [Range(1, int.MaxValue)]
    int SportId,

    [Required]
    DateTime StartedAt,

    // Up to 48 hours
    [Range(1, 172800)]
    int? DurationSeconds,

    [Range(0.01, 1000)]
    double? DistanceKm,

    [Range(30, 230)]
    int? AvgHeartRate,

    [Range(0, 20000)]
    int? Calories,

    [Range(1, 10)]
    int? PerceivedEffort,

    [MaxLength(1000)]
    string? Notes,

    // Only for composite sports; the list order becomes SegmentOrder (1-based)
    List<WorkoutSegmentRequest>? Segments);

public record WorkoutSegmentRequest(
    [Range(1, int.MaxValue)]
    int DisciplineId,

    [Range(1, 172800)]
    int? DurationSeconds,

    [Range(0.01, 1000)]
    double? DistanceKm);
