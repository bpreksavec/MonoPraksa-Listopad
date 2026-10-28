using WorkoutTracker.DAL.Models;

namespace WorkoutTracker.Service.DTOs.Sports;

public record SportResponse(
    int Id,
    string Name,
    bool IsComposite,
    List<SportSegmentResponse> Segments)
{
    public static SportResponse FromEntity(Sport sport) => new(
        sport.Id,
        sport.Name,
        sport.IsComposite,
        sport.Segments
            .OrderBy(s => s.SegmentOrder)
            .Select(s => new SportSegmentResponse(s.SegmentOrder, s.DisciplineId, s.Discipline.Name, s.DistanceKm))
            .ToList());
}

public record SportSegmentResponse(
    int SegmentOrder,
    int DisciplineId,
    string DisciplineName,
    double? DistanceKm);
