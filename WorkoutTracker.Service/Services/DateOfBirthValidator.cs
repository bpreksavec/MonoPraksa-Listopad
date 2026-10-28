using WorkoutTracker.Exceptions;

namespace WorkoutTracker.Service.Services;

// Shared by registration and profile updates
public static class DateOfBirthValidator
{
    private const int MaxAgeYears = 120;

    /// <exception cref="ValidationFailedException">Date is in the future or implausibly far in the past.</exception>
    public static void Validate(DateOnly? dateOfBirth, string field = "DateOfBirth")
    {
        if (dateOfBirth is null)
        {
            return;
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (dateOfBirth > today)
        {
            throw new ValidationFailedException(field, "Date of birth cannot be in the future.");
        }

        if (dateOfBirth < today.AddYears(-MaxAgeYears))
        {
            throw new ValidationFailedException(field, $"Date of birth cannot be more than {MaxAgeYears} years ago.");
        }
    }
}
