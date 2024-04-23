namespace HangFire.Dtos
{
    public class RecurringJobRespDto
    {
        public string Id { get; set; }

        public string Cron { get; set; }

        public string Queue { get; set; }

        public DateTime? NextExecution { get; set; }

        public DateTime? LastExecution { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool Removed { get; set; }
    }
}
