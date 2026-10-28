using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Service.DTOs.Sports;
using WorkoutTracker.Service.Interfaces;

namespace WorkoutTracker.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class SportController : ControllerBase
{
    private readonly ISportService _service;

    public SportController(ISportService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SportResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }
}
