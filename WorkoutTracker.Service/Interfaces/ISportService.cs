using WorkoutTracker.Service.DTOs.Sports;

namespace WorkoutTracker.Service.Interfaces;

public interface ISportService
{
    Task<List<SportResponse>> GetAllAsync();
}
