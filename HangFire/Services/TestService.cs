namespace HangFire.Services
{
    public class TestService : ITestService
    {
        private readonly ILogger _logger;
        public TestService(ILogger<TestService> logger) 
        { 
            _logger = logger;
        }
        public void HelloWorld(string outputString)
        {
            Console.WriteLine(outputString);
        }

        public async Task LongRunningMethod(CancellationToken token)
        {
            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine("LongRunningMethod");
                await Task.Delay(TimeSpan.FromSeconds(1), token);
            }
        }

        public void WriteLog(string message)
        {
            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd hh:mm:ss tt} {message}");
        }
    }
}
