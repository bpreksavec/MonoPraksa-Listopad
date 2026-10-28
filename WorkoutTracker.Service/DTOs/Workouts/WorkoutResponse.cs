using WorkoutTracker.DAL.Models;

namespace WorkoutTracker.Service.DTOs.Workouts;

public record WorkoutResponse(
    Guid Id,
    Guid UserId,
    int SportId,
    string SportName,
    DateTime StartedAt,
    int? DurationSeconds,
    double? DistanceKm,
    int? AvgHeartRate,
    int? Calories,
    int? PerceivedEffort,
    string? Notes,
    DateTime CreatedAt,
    List<WorkoutSegmentResponse> Segments)
{
    public static WorkoutResponse FromEntity(Workout workout) => new(
        workout.Id,
        workout.UserId,
        workout.SportId,
        workout.Sport.Name,
        workout.StartedAt,
        workout.DurationSeconds,
        workout.DistanceKm,
        workout.AvgHeartRate,
        workout.Calories,
        workout.PerceivedEffort,
        workout.Notes,
        workout.CreatedAt,
        workout.Segments
            .OrderBy(s => s.SegmentOrder)
            .Select(s => new WorkoutSegmentResponse(
                s.SegmentOrder, s.DisciplineId, s.Discipline.Name, s.DurationSeconds, s.DistanceKm))
            .ToList());
}

public record WorkoutSegmentResponse(
    int SegmentOrder,
    int DisciplineId,
    string DisciplineName,
    int? DurationSeconds,
    double? DistanceKm);
