namespace WorkoutTracker.Service.DTOs;

/// <summary>The authenticated caller, used for ownership checks.</summary>
public record CurrentUser(Guid Id, bool IsAdmin);
