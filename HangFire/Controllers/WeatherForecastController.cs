using Hangfire;
using HangFire.Services;
using Microsoft.AspNetCore.Mvc;

namespace HangFire.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        public ITestService testService { get; set; }

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITestService testService)
        {
            _logger = logger;
            this.testService = testService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            //RecurringJob.AddOrUpdate("weatherjob", () => Console.WriteLine("GetWeatherForecast recurring job start!"), Cron.Minutely);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        [Route("CreateBackgroundJob")]
        public ActionResult CreateBackgroundJob()
        {
            //BackgroundJob.Enqueue(() => Console.WriteLine("Background Job triggered"));
            BackgroundJob.Enqueue<ITestService>(x => x.WriteLog("Background Job triggered"));
            return Ok();
        }

        [HttpPost]
        [Route("CreateScheduleJob")]
        public ActionResult CreateScheduleJob()
        {
            var scheduleTime = DateTime.Now.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleTime);
            //BackgroundJob.Schedule(() => Console.WriteLine("Scheduled Job triggered"), dateTimeOffset);
            BackgroundJob.Schedule<ITestService>(x => x.WriteLog("Scheduled Job triggered"), dateTimeOffset);
            return Ok();
        }

        [HttpPost]
        [Route("CreateContinuationJob")]
        public ActionResult CreateContinuationJob()
        {
            var scheduleTime = DateTime.Now.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleTime);
            var jobId = BackgroundJob.Schedule<ITestService>(x => x.WriteLog("Scheduled Job triggered"), dateTimeOffset);
            var jobId2 = BackgroundJob.ContinueJobWith<ITestService>(jobId, x => x.WriteLog("Continue job 1 triggered"));
            var jobId3 = BackgroundJob.ContinueJobWith<ITestService>(jobId2, x => x.WriteLog("Continue job 2 triggered"));
            return Ok();
        }

        [HttpPost]
        [Route("CreateRecurringJob")]
        public ActionResult CreateRecurringJob()
        {
            var specificTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16, 30, 0);
            RecurringJob.AddOrUpdate<ITestService>("recurringJob1", x => x.WriteLog("Recurring job triggered"), "* * * * *");//every minute
            //RecurringJob.AddOrUpdate<ITestService>("recurringJob2", x => x.WriteLog("Recurring job triggered"), Cron.Hourly(0));//every hour start from 00 minute
            return Ok();
        }

        [HttpPost]
        [Route("RemoveRecurringJob")]
        public ActionResult RemoveRecurringJob(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
            return Ok();
        }

        [HttpPost]
        [Route("LongRunMethod")]
        public ActionResult LongRunMethod(string jobId)
        {
            BackgroundJob.Enqueue<ITestService>(x => x.LongRunningMethod(CancellationToken.None));
            return Ok();
        }
    }
}
