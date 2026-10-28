using WorkoutTracker.DAL.Interfaces;
using WorkoutTracker.Service.DTOs.Sports;
using WorkoutTracker.Service.Interfaces;

namespace WorkoutTracker.Service.Services;

public class SportService : ISportService
{
    private readonly ISportRepository _repository;

    public SportService(ISportRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SportResponse>> GetAllAsync()
    {
        var sports = await _repository.GetAllAsync();
        return sports.Select(SportResponse.FromEntity).ToList();
    }
}
