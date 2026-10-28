using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Api.Extensions;
using WorkoutTracker.Service.DTOs.BodyMeasurements;
using WorkoutTracker.Service.Interfaces;

namespace WorkoutTracker.Api.Controllers;

// Always acts on the logged-in user's own measurements
[ApiController]
[Route("[controller]")]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class BodyMeasurementController : ControllerBase
{
    private readonly IBodyMeasurementService _service;

    public BodyMeasurementController(IBodyMeasurementService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<BodyMeasurementResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync(User.GetUserId()));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BodyMeasurementResponse>> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id, User.GetUserId()));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BodyMeasurementResponse>> Create(BodyMeasurementRequest request)
    {
        var created = await _service.AddAsync(User.GetUserId(), request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, BodyMeasurementRequest request)
    {
        await _service.UpdateAsync(id, User.GetUserId(), request);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id, User.GetUserId());
        return NoContent();
    }
}
