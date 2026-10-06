using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkoutController : ControllerBase
    {
        private static List<Workout> workouts = new List<Workout>
        {
            new Workout
            {
                Id = 1,
                Date = DateTime.Now.AddDays(-2),
                DistanceKm = 5,
                DurationMinutes = 25,
                Type = "Running"
            },

            new Workout
            {
                Id = 2,
                Date = DateTime.Now.AddDays(-1),
                DistanceKm = 10,
                DurationMinutes = 52,
                Type = "Running"
            }
        };

        [HttpGet]
        public IEnumerable<Workout> Get()
        {
            return workouts;
        }

        [HttpGet("{Id}")]
        public ActionResult<Workout> GetById(int Id)
        {
            var workout = workouts.FirstOrDefault(w => w.Id == Id);

            if (workout == null)
            {
                return NotFound();
            }

            return workout;
        }

        [HttpPost]
        public ActionResult<Workout> Create(Workout workout)
        {
            workout.Id = workouts.Count + 1;

            workouts.Add(workout);

            return workout;
        }

        [HttpPut("{Id}")]
        public ActionResult<Workout> Update(int Id, Workout updatedWorkout)
        {
            var workout = workouts.FirstOrDefault(w => w.Id == Id);

            if (workout == null)
            {
                return NotFound();
            }

            workout.Date = updatedWorkout.Date;
            workout.DistanceKm = updatedWorkout.DistanceKm;
            workout.DurationMinutes = updatedWorkout.DurationMinutes;
            workout.Type = updatedWorkout.Type;

            return workout;
        }

        [HttpDelete("{Id}")]
        public ActionResult Delete(int Id)
        {
            var workout = workouts.FirstOrDefault(w => w.Id == Id);

            if (workout == null)
            {
                return NotFound();
            }

            workouts.Remove(workout);

            return NoContent();
        }


    }
}