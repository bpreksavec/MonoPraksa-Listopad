namespace WorkoutTracker.DAL.Models;

public class WorkoutSegment
{
    public Guid WorkoutId { get; set; }
    public int SegmentOrder { get; set; }
    public int DisciplineId { get; set; }
    public int? DurationSeconds { get; set; }
    public double? DistanceKm { get; set; }

    public Workout Workout { get; set; } = null!;
    public Sport Discipline { get; set; } = null!;
}
