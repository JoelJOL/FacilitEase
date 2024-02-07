namespace FacilitEase.Models.ApiModels
{
    public class WeekReport
    {
        public int DailyTickets { get; set; }
        public int DailyResolved {  get; set; }
        public int DailyUnresolved { get; set; }
        public int DailyEscalated { get; set; }
        public int WeeklyTickets { get; set; }
        public int WeeklyResolved { get; set; }
        public int WeeklyUnresolved { get; set; }
        public int WeeklyEscalated { get; set; }
    }
}
