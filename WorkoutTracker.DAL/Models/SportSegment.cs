namespace WorkoutTracker.DAL.Models;

public class SportSegment
{
    public int CompositeSportId { get; set; }
    public int SegmentOrder { get; set; }
    public int DisciplineId { get; set; }
    public double? DistanceKm { get; set; }

    public Sport CompositeSport { get; set; } = null!;
    public Sport Discipline { get; set; } = null!;
}
