using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkoutController : ControllerBase
    {
        private readonly WorkoutService _service;
        private readonly WorkoutValidator _validator;
        private readonly WorkoutStatistics _statistics;

        public WorkoutController(WorkoutService service,
                                WorkoutValidator validator,
                                WorkoutStatistics statistics)
        {
            _service = service;
            _validator = validator;
            _statistics = statistics;
        }

        [HttpGet]
        public ActionResult<List<Workout>> Get()
        {
            var requestNumber = _statistics.IncrementRequests();

            Console.WriteLine($"Request number: {requestNumber}");

            return _service.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Workout> GetById(int id)
        {
            var workout = _service.GetById(id);

            if (workout == null)
            {
                return NotFound();
            }

            return workout;
        }

        [HttpPost]
        public ActionResult<Workout> Create(Workout workout)
        {
            if (!_validator.IsValid())
            {
                return BadRequest();
            }

            var createdWorkout = _service.Create(workout);

            return createdWorkout;
        }

        [HttpPut("{id}")]
        public ActionResult<Workout> Update(int id, Workout updatedWorkout)
        {
            var workout = _service.Update(id, updatedWorkout);

            if (workout == null)
            {
                return NotFound();
            }

            return workout;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var deleted = _service.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}