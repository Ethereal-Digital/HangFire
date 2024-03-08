namespace HangFire.Services
{
    public interface ITestService
    {
        public Task LongRunningMethod(CancellationToken token);

        public void WriteLog(string message);
    }
}
