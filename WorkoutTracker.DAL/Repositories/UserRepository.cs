using Microsoft.EntityFrameworkCore;
using WorkoutTracker.DAL.Interfaces;
using WorkoutTracker.DAL.Models;

namespace WorkoutTracker.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Profile)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Profile)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<Role?> GetRoleByNameAsync(string name)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateProfileAsync(Guid id, User user)
    {
        var existingUser = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (existingUser == null)
        {
            return false;
        }

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.DateOfBirth = user.DateOfBirth;
        existingUser.Gender = user.Gender;

        var profile = user.Profile ?? new UserProfile();

        if (existingUser.Profile == null)
        {
            _context.UserProfiles.Add(new UserProfile
            {
                UserId = id,
                Bio = profile.Bio,
                ProfileImageUrl = profile.ProfileImageUrl
            });
        }
        else
        {
            existingUser.Profile.Bio = profile.Bio;
            existingUser.Profile.ProfileImageUrl = profile.ProfileImageUrl;
        }

        await _context.SaveChangesAsync();

        return true;
    }
}
