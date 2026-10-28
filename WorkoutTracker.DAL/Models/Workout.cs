namespace WorkoutTracker.DAL.Models;

public class Workout
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int SportId { get; set; }
    public DateTime StartedAt { get; set; }
    public int? DurationSeconds { get; set; }
    public double? DistanceKm { get; set; }
    public int? AvgHeartRate { get; set; }
    public int? Calories { get; set; }
    public int? PerceivedEffort { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Sport Sport { get; set; } = null!;
    public ICollection<WorkoutSegment> Segments { get; set; } = new List<WorkoutSegment>();
}
