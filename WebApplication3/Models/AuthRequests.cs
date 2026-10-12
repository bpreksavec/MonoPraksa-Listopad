using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models;

public record RegisterRequest(
    [Required, MinLength(3), MaxLength(50)] string Username,
    [Required, EmailAddress] string Email,
    [Required, MinLength(8)] string Password);

public record LoginRequest(
    [Required] string Username,
    [Required] string Password);