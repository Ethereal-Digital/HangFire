using Hangfire;
using Hangfire.Storage;
using HangFire.Dtos;
using HangFire.Extensions;
using Microsoft.Extensions.Logging;

namespace HangFire.Services
{
    public class HangfireService : IHangfireService
    {
        public string eInvoiceServerUrl { get; set; }
        public HangfireService(IConfiguration configuration) 
        {
            eInvoiceServerUrl = configuration.GetValue<string>("eInvoiceServer");
        }

        public void CreateRecurringJob(CreateRecurringJobDto request)
        {
            try
            {
                //var specificTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16, 30, 0);
                string apiUrl = eInvoiceServerUrl + request.APIUrl;
                string cronExpression = GenerateCronExpression(request.Minute, request.Hour, request.DayOfMonth, request.Month, request.DayOfWeek, request.Year);
                string data = "";
                if (request.HttpMethod == "POST")
                    RecurringJob.AddOrUpdate(request.SchedularName, () => HttpClientExtensions.PostAsJson(apiUrl, data, null), cronExpression);
                else if (request.HttpMethod == "PUT")
                    RecurringJob.AddOrUpdate(request.SchedularName, () => HttpClientExtensions.PutAsJson(apiUrl, data, null), cronExpression);
                else if (request.HttpMethod == "GET")
                    RecurringJob.AddOrUpdate(request.SchedularName, () => HttpClientExtensions.GetAsJson(apiUrl, null), cronExpression);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            

        }

        public void RemoveJob(string jobId)
        {
            try
            {
                RecurringJob.RemoveIfExists(jobId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public List<RecurringJobRespDto> GetAllRecurringJob()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                var recurringJobs = connection.GetRecurringJobs();
                return recurringJobs.Select(x => new RecurringJobRespDto()
                {
                    Id = x.Id,
                    Cron = x.Cron,
                    Queue = x.Queue,
                    NextExecution = x.NextExecution,
                    LastExecution = x.LastExecution,
                    CreatedAt = x.CreatedAt,
                    Removed = x.Removed
                }).ToList();
            }
        }


        static string GenerateCronExpression(string minute, string hour, string dayOfMonth, string month, string dayOfWeek, string year)
        {
            string[] parts = { minute, hour, dayOfMonth, month, dayOfWeek };
            string cronExpression = string.Join(" ", parts);
            return cronExpression;
        }
    }
}
