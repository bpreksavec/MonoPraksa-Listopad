using System.ComponentModel.DataAnnotations;
using WorkoutTracker.Common.Enums;
using WorkoutTracker.DAL.Models;

namespace WorkoutTracker.Service.DTOs.Profile;

public record ProfileRequest(
    [Required, StringLength(50, MinimumLength = 1)]
    [RegularExpression(ValidationPatterns.LettersOnly, ErrorMessage = ValidationPatterns.LettersOnlyMessage)]
    string FirstName,

    [Required, StringLength(50, MinimumLength = 1)]
    [RegularExpression(ValidationPatterns.LettersOnly, ErrorMessage = ValidationPatterns.LettersOnlyMessage)]
    string LastName,

    DateOnly? DateOfBirth,

    Gender? Gender,

    [MaxLength(1000)]
    string? Bio,

    [Url, MaxLength(500)]
    string? ProfileImageUrl);

public record ProfileResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    DateOnly? DateOfBirth,
    Gender? Gender,
    DateTime CreatedAt,
    string? Bio,
    string? ProfileImageUrl,
    List<string> Roles)
{
    public static ProfileResponse FromEntity(User user) => new(
        user.Id,
        user.FirstName,
        user.LastName,
        user.Email,
        user.DateOfBirth,
        user.Gender,
        user.CreatedAt,
        user.Profile?.Bio,
        user.Profile?.ProfileImageUrl,
        user.UserRoles.Select(ur => ur.Role.Name).ToList());
}
