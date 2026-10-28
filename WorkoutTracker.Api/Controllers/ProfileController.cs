using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Api.Extensions;
using WorkoutTracker.DAL.Models;
using WorkoutTracker.Service.DTOs.Profile;
using WorkoutTracker.Service.Interfaces;

namespace WorkoutTracker.Api.Controllers;

// GET/PUT act on the logged-in user's own profile; listing and deleting are admin-only
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

    [HttpGet("all")]
    [Authorize(Roles = Role.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<ProfileResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    // Deletes the whole user account, including their workouts and measurements
    [HttpDelete("{userId:guid}")]
    [Authorize(Roles = Role.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid userId)
    {
        await _service.DeleteAsync(userId, User.GetUserId());
        return NoContent();
    }
}
