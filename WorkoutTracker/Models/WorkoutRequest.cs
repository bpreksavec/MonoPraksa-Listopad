using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models;

public record WorkoutRequest(
    [Required, StringLength(50, MinimumLength = 2)]
    string Sport,

    [Range(0.01, 1000)]
    double? Distance,

    [Range(1, 1440)]
    int? Duration,

    [Range(30, 230)]
    int? AvgHeartrate,

    Guid? AthleteId);