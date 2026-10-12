using WebApplication3.Models;

namespace WebApplication3.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ExistsAsync(string username, string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}