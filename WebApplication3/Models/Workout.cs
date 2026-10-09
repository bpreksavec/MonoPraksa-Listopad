namespace WebApplication3.Models;

public partial class Workout
{
    public Guid Id { get; set; }

    public string? Sport { get; set; }

    public double? Distance { get; set; }

    public int? Duration { get; set; }

    public int? AvgHeartrate { get; set; }

    public Guid? AthleteId { get; set; }

    public virtual Athlete? Athlete { get; set; }
}