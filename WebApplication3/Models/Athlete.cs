namespace WebApplication3.Models;

public partial class Athlete
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Country { get; set; }

    public int? Age { get; set; }

    public double? Height { get; set; }

    public double? Weight { get; set; }

    public virtual ICollection<Workout> Workouts { get; set; } = new List<Workout>();
}