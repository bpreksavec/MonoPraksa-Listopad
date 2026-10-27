using WorkoutTracker.Exceptions;
using WorkoutTracker.Models;

namespace WorkoutTracker.Services;

public class WorkoutValidator
{
    private static readonly HashSet<string> AllowedSports = new(StringComparer.OrdinalIgnoreCase)
    {
        "Running", "Cycling", "Swimming", "Walking", "Hiking", "Rowing", "Strength", "Other"
    };

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

    public void Validate(WorkoutRequest request)
    {
        var errors = new Dictionary<string, List<string>>();

        void Add(string field, string message)
        {
            if (!errors.TryGetValue(field, out var list))
                errors[field] = list = new List<string>();
            list.Add(message);
        }

        var sport = request.Sport?.Trim() ?? string.Empty;

        if (!AllowedSports.Contains(sport))
        {
            Add(nameof(request.Sport),
                $"Sport must be one of: {string.Join(", ", AllowedSports)}.");
        }

        if (request.Distance is null && request.Duration is null)
        {
            Add(nameof(request.Duration), "Either distance or duration must be provided.");
        }

        if (request.Distance is > 0 && request.Duration is > 0
            && MaxSpeedKmh.TryGetValue(sport, out var maxSpeed))
        {
            var speed = request.Distance.Value / (request.Duration.Value / 60.0);
            if (speed > maxSpeed)
            {
                Add(nameof(request.Distance),
                    $"An average speed of {speed:F1} km/h is not realistic for {sport}.");
            }
        }

        if (request.AvgHeartrate is not null && request.Duration is null)
        {
            Add(nameof(request.Duration), "Duration is required when average heart rate is provided.");
        }

        if (request.AthleteId == Guid.Empty)
        {
            Add(nameof(request.AthleteId), "AthleteId cannot be an empty GUID.");
        }

        if (errors.Count > 0)
        {
            throw new ValidationFailedException(
                errors.ToDictionary(e => e.Key, e => e.Value.ToArray()));
        }
    }
}