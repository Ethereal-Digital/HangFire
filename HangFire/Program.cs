using Hangfire;
using Hangfire.SqlServer;
using HangFire.Services;
using HangfireBasicAuthenticationFilter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Hangfire services.
var connString = builder.Configuration.GetConnectionString("DataStoreContext");
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(connString));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

builder.Services.AddScoped<ITestService, TestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Hangfire dashboard demo",
    DarkModeEnabled = false,
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            User = "admin",
            Pass = "admin123"
        }
    }
});

//BackgroundJob.Enqueue<ITestService>(x => x.LongRunningMethod(CancellationToken.None));

//BackgroundJob.Schedule(
//    () => Console.WriteLine("Hello, world with delay 15 seconds"),
//    TimeSpan.FromSeconds(15));

//RecurringJob.AddOrUpdate("easyjob", () => Console.WriteLine("Easy recurrence!"), Cron.Minutely);

app.UseAuthorization();

app.MapControllers();

app.Run();
