namespace WorkoutTracker.DAL.Models;

public class BodyMeasurement
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public double? HeightCm { get; set; }
    public double? WeightKg { get; set; }
    public DateTime MeasuredAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}
