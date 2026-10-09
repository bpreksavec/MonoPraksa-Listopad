using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkoutController : ControllerBase
{
    private readonly WorkoutService _service;

    public WorkoutController(WorkoutService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Workout>>> GetAll()
    {
        var workouts = await _service.GetAllAsync();

        return Ok(workouts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Workout>> GetById(Guid id)
    {
        var workout = await _service.GetByIdAsync(id);

        if (workout == null)
        {
            return NotFound();
        }

        return Ok(workout);
    }

    [HttpPost]
    public async Task<ActionResult<Workout>> Create(Workout workout)
    {
        var createdWorkout = await _service.AddAsync(workout);

        return Ok(createdWorkout);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Workout workout)
    {
        var updated = await _service.UpdateAsync(id, workout);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}