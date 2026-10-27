using WorkoutTracker.Models;

namespace WorkoutTracker.Services;

public interface ITokenService
{
    string CreateToken(User user);
}