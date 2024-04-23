namespace HangFire.Dtos
{
    public class CreateRecurringJobDto
    {
        public string SchedularName { get; set; }
        public string HttpMethod { get; set; }
        public string APIUrl { get; set; }
        public DateTime StartDateTime { get; set; }
        public string Minute { get; set; }
        public string Hour { get; set; }
        public string DayOfMonth { get; set; }
        public string Month { get; set; }
        public string DayOfWeek { get; set; }
        public string Year { get; set; }

    }
}
