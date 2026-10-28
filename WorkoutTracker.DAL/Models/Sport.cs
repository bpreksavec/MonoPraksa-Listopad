namespace WorkoutTracker.DAL.Models;

public class Sport
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Composite sports (e.g. triathlon) are made of ordered segments of other sports
    public bool IsComposite { get; set; }

    public ICollection<SportSegment> Segments { get; set; } = new List<SportSegment>();
}
