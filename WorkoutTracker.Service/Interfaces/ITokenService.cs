using WorkoutTracker.Models;

namespace WorkoutTracker.Service.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}