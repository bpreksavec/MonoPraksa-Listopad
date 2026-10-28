using WorkoutTracker.DAL.Models;

namespace WorkoutTracker.DAL.Interfaces;

public interface ISportRepository
{
    Task<List<Sport>> GetAllAsync();
    Task<Sport?> GetByIdAsync(int id);
    Task<List<Sport>> GetByIdsAsync(IEnumerable<int> ids);
}
