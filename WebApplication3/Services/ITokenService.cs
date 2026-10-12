using WebApplication3.Models;

namespace WebApplication3.Services;

public interface ITokenService
{
    string CreateToken(User user);
}