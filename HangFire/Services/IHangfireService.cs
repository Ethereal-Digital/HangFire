using Hangfire.Storage;
using HangFire.Dtos;

namespace HangFire.Services
{
    public interface IHangfireService
    {
        public void CreateRecurringJob(CreateRecurringJobDto request);
        public void RemoveJob(string jobId);
        public List<RecurringJobRespDto> GetAllRecurringJob();
    }
}
