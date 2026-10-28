using Microsoft.EntityFrameworkCore;
using WorkoutTracker.DAL.Interfaces;
using WorkoutTracker.DAL.Models;

namespace WorkoutTracker.DAL.Repositories;

public class SportRepository : ISportRepository
{
    private readonly AppDbContext _context;

    public SportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sport>> GetAllAsync()
    {
        return await _context.Sports
            .AsNoTracking()
            .Include(s => s.Segments.OrderBy(seg => seg.SegmentOrder))
                .ThenInclude(seg => seg.Discipline)
            .OrderBy(s => s.Id)
            .ToListAsync();
    }

    public async Task<Sport?> GetByIdAsync(int id)
    {
        return await _context.Sports
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Sport>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids.Distinct().ToList();

        return await _context.Sports
            .AsNoTracking()
            .Where(s => idList.Contains(s.Id))
            .ToListAsync();
    }
}
