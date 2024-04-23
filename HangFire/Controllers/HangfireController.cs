using HangFire.Services;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HangFire.Dtos;
using Azure.Core;
using HangFire.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HangFire.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        private readonly IHangfireService hangfireService;
        public HangfireController(IHangfireService _hangfireService) 
        {
            hangfireService = _hangfireService;
        }

        [HttpPost]
        public ActionResult CreateScheduleJob()
        {
            var scheduleTime = DateTime.Now.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleTime);
            //BackgroundJob.Schedule(() => Console.WriteLine("Scheduled Job triggered"), dateTimeOffset);
            BackgroundJob.Schedule<ITestService>(x => x.WriteLog("Scheduled Job triggered"), dateTimeOffset);
            return Ok();
        }

        //[HttpPost]
        //[Route("CreateContinuationJob")]
        //public ActionResult CreateContinuationJob()
        //{
        //    var scheduleTime = DateTime.Now.AddSeconds(5);
        //    var dateTimeOffset = new DateTimeOffset(scheduleTime);
        //    var jobId = BackgroundJob.Schedule<ITestService>(x => x.WriteLog("Scheduled Job triggered"), dateTimeOffset);
        //    var jobId2 = BackgroundJob.ContinueJobWith<ITestService>(jobId, x => x.WriteLog("Continue job 1 triggered"));
        //    var jobId3 = BackgroundJob.ContinueJobWith<ITestService>(jobId2, x => x.WriteLog("Continue job 2 triggered"));
        //    return Ok();
        //}

        [HttpPost]
        public ActionResult CreateRecurringJob(CreateRecurringJobDto request)
        {
            hangfireService.CreateRecurringJob(request);
            return Ok();
        }

        [HttpDelete]
        public ActionResult RemoveJob(string jobId)
        {
            hangfireService.RemoveJob(jobId);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> TestCallAPI()
        {
            try
            {
                await HttpClientExtensions.PostAsJson("https://localhost:7283/api/EInvSubBatch/GetSubmissionStatus", "", null);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet]
        public IActionResult GetRecurringJob()
        {
            var resp = hangfireService.GetAllRecurringJob();
            return Ok(resp);
        }
    }
}
