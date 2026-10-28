using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Api.Extensions;
using WorkoutTracker.Service.DTOs.Profile;
using WorkoutTracker.Service.Interfaces;

namespace WorkoutTracker.Api.Controllers;

// Always acts on the logged-in user's own profile
[ApiController]
[Route("[controller]")]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _service;

    public ProfileController(IProfileService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileResponse>> Get()
    {
        return Ok(await _service.GetAsync(User.GetUserId()));
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileResponse>> Update(ProfileRequest request)
    {
        return Ok(await _service.UpdateAsync(User.GetUserId(), request));
    }
}
