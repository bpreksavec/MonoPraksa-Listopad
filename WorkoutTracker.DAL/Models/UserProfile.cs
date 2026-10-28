namespace WorkoutTracker.DAL.Models;

public class UserProfile
{
    public Guid UserId { get; set; }
    public string? Bio { get; set; }
    public string? ProfileImageUrl { get; set; }

    public User User { get; set; } = null!;
}
