using System.ComponentModel.DataAnnotations;
using WorkoutTracker.Common.Enums;

namespace WorkoutTracker.Service.DTOs.Auth;

public record RegisterRequest(
    [Required, StringLength(50, MinimumLength = 1)]
    string FirstName,

    [Required, StringLength(50, MinimumLength = 1)]
    string LastName,

    [Required, EmailAddress, MaxLength(256)]
    string Email,

    [Required, StringLength(100, MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Password must contain a lowercase letter, an uppercase letter and a digit.")]
    string Password,

    DateOnly? DateOfBirth,

    Gender? Gender);

public record LoginRequest(
    [Required, EmailAddress, MaxLength(256)] string Email,
    [Required, MaxLength(100)] string Password);
