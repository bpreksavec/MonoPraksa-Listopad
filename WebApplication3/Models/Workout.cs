namespace WebApplication3.Models
{
    public class Workout
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public double DistanceKm { get; set; }

        public int DurationMinutes { get; set; }

        public string Type { get; set; } = "";
    }
}