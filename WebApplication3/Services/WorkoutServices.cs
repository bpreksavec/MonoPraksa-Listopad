using WebApplication3.Models;
using WebApplication3.Repositories;

namespace WebApplication3.Services
{
    public class WorkoutService
    {
        private readonly WorkoutRepository _repository;

        public WorkoutService(WorkoutRepository repository)
        {
            _repository = repository;
        }

        public List<Workout> GetAll()
        {
            return _repository.GetAll();
        }

        public Workout? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public Workout Create(Workout workout)
        {
            return _repository.Create(workout);
        }

        public Workout? Update(int id, Workout updatedWorkout)
        {
            return _repository.Update(id, updatedWorkout);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
    }
}