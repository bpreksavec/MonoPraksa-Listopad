namespace WebApplication3.Services
{
    public class WorkoutStatistics
    {
        public int TotalRequests { get; set; }

        public int IncrementRequests()
        {
            TotalRequests++;
            return TotalRequests;
        }
    }
}