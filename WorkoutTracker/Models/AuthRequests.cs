using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models;

public record RegisterRequest(
    [Required, StringLength(50, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9_.-]+$",
        ErrorMessage = "Username may only contain letters, digits, '_', '.' and '-'.")]
    string Username,

    [Required, EmailAddress, MaxLength(256)]
    string Email,

    [Required, StringLength(100, MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Password must contain a lowercase letter, an uppercase letter and a digit.")]
    string Password);

public record LoginRequest(
    [Required, MaxLength(50)] string Username,
    [Required, MaxLength(100)] string Password);