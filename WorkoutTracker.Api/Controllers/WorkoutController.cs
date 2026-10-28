using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Api.Extensions;
using WorkoutTracker.Service.DTOs.Workouts;
using WorkoutTracker.Service.Interfaces;

namespace WorkoutTracker.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class WorkoutController : ControllerBase
{
    private readonly IWorkoutService _service;

    public WorkoutController(IWorkoutService service)
    {
        _service = service;
    }

    // Users get their own workouts, admins get everyone's
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<WorkoutResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync(User.ToCurrentUser()));
    }

    // :guid constraint → a non-GUID id returns 404 instead of reaching the action
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutResponse>> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id, User.ToCurrentUser()));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WorkoutResponse>> Create(WorkoutRequest request)
    {
        var created = await _service.AddAsync(User.ToCurrentUser(), request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, WorkoutRequest request)
    {
        await _service.UpdateAsync(id, User.ToCurrentUser(), request);
        return NoContent();
    }

    // Owners can delete their own workouts, admins can delete any
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id, User.ToCurrentUser());
        return NoContent();
    }}
