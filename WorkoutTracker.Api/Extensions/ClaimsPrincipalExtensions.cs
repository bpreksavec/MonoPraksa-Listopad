using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using WorkoutTracker.DAL.Models;
using WorkoutTracker.Service.DTOs;

namespace WorkoutTracker.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    // MapInboundClaims = false keeps the JWT "sub" claim name as-is
    public static Guid GetUserId(this ClaimsPrincipal principal) =>
        Guid.Parse(principal.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

    public static CurrentUser ToCurrentUser(this ClaimsPrincipal principal) =>
        new(principal.GetUserId(), principal.IsInRole(Role.Admin));
}
