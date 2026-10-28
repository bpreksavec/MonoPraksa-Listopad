using WorkoutTracker.DAL.Models;
using WorkoutTracker.Exceptions;
using WorkoutTracker.Service.DTOs.Workouts;

namespace WorkoutTracker.Service.Services;

public class WorkoutValidator
{
    // Max plausible speed in km/h per sport, to catch typos like 100 km in 10 min
    private static readonly Dictionary<string, double> MaxSpeedKmh = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Running"] = 30,
        ["Walking"] = 10,
        ["Hiking"] = 10,
        ["Swimming"] = 10,
        ["Rowing"] = 25,
        ["Cycling"] = 80
    };

    /// <param name="sports">Sports referenced by the request (workout sport and segment disciplines), keyed by id.</param>
    public void Validate(WorkoutRequest request, IReadOnlyDictionary<int, Sport> sports)
    {
        var errors = new Dictionary<string, List<string>>();

        void Add(string field, string message)
        {
            if (!errors.TryGetValue(field, out var list))
                errors[field] = list = new List<string>();
            list.Add(message);
        }

        sports.TryGetValue(request.SportId, out var sport);

        if (sport == null)
        {
            Add(nameof(request.SportId), $"Sport with id '{request.SportId}' does not exist.");
        }

        if (request.StartedAt.ToUniversalTime() > DateTime.UtcNow.AddMinutes(5))
        {
            Add(nameof(request.StartedAt), "A workout cannot start in the future.");
        }

        if (request.DistanceKm is null && request.DurationSeconds is null)
        {
            Add(nameof(request.DurationSeconds), "Either distance or duration must be provided.");
        }

        if (sport != null)
        {
            CheckSpeed(nameof(request.DistanceKm), sport.Name, request.DistanceKm, request.DurationSeconds, Add);
        }

        if (request.AvgHeartRate is not null && request.DurationSeconds is null)
        {
            Add(nameof(request.DurationSeconds), "Duration is required when average heart rate is provided.");
        }

        var segments = request.Segments ?? [];

        if (segments.Count > 0 && sport is { IsComposite: false })
        {
            Add(nameof(request.Segments), $"Segments are only allowed for composite sports, and {sport.Name} is not one.");
        }

        for (var i = 0; i < segments.Count; i++)
        {
            var segment = segments[i];
            var field = $"{nameof(request.Segments)}[{i}]";

            if (!sports.TryGetValue(segment.DisciplineId, out var discipline))
            {
                Add(field, $"Sport with id '{segment.DisciplineId}' does not exist.");
                continue;
            }

            if (discipline.IsComposite)
            {
                Add(field, "A segment discipline cannot itself be a composite sport.");
            }

            if (segment.DistanceKm is null && segment.DurationSeconds is null)
            {
                Add(field, "Either distance or duration must be provided.");
            }

            CheckSpeed(field, discipline.Name, segment.DistanceKm, segment.DurationSeconds, Add);
        }

        if (request.DurationSeconds is not null
            && segments.Sum(s => s.DurationSeconds ?? 0) > request.DurationSeconds)
        {
            Add(nameof(request.Segments), "Segment durations add up to more than the workout duration.");
        }

        if (errors.Count > 0)
        {
            throw new ValidationFailedException(
                errors.ToDictionary(e => e.Key, e => e.Value.ToArray()));
        }
    }

    private static void CheckSpeed(string field, string sportName, double? distanceKm, int? durationSeconds,
        Action<string, string> add)
    {
        if (distanceKm is > 0 && durationSeconds is > 0
            && MaxSpeedKmh.TryGetValue(sportName, out var maxSpeed))
        {
            var speed = distanceKm.Value / (durationSeconds.Value / 3600.0);
            if (speed > maxSpeed)
            {
                add(field, $"An average speed of {speed:F1} km/h is not realistic for {sportName}.");
            }
        }
    }
}
