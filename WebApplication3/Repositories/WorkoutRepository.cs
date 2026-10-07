using WebApplication3.Models;

namespace WebApplication3.Repositories
{
    public class WorkoutRepository
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

        public List<Workout> GetAll()
        {
            return workouts;
        }

        public Workout? GetById(int id)
        {
            return workouts.FirstOrDefault(w => w.Id == id);
        }

        public Workout Create(Workout workout)
        {
            workout.Id = workouts.Count + 1;

            workouts.Add(workout);

            return workout;
        }

        public Workout? Update(int id, Workout updatedWorkout)
        {
            var workout = workouts.FirstOrDefault(w => w.Id == id);

            if (workout == null)
            {
                return null;
            }

            workout.Date = updatedWorkout.Date;
            workout.DistanceKm = updatedWorkout.DistanceKm;
            workout.DurationMinutes = updatedWorkout.DurationMinutes;
            workout.Type = updatedWorkout.Type;

            return workout;
        }

        public bool Delete(int id)
        {
            var workout = workouts.FirstOrDefault(w => w.Id == id);

            if (workout == null)
            {
                return false;
            }

            workouts.Remove(workout);

            return true;
        }
    }
}