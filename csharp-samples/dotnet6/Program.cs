using Coravel;
using dotnet6;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.AddRepo361(configuration.GetSection("Repo361API"));
services.AddTransient<Repo361MySQLService>();
services.AddTransient<Repo361ScheduleInvocable>();
services.AddScheduler();

var app = builder.Build();

app.Services.UseScheduler(scheduler =>
{
    //Repo raw data available at 05.00 - utc time 
    //The scheduler uses UTC time by default
    scheduler.Schedule<Repo361ScheduleInvocable>().DailyAtHour(6);
});

app.MapGet("/test", async (IServiceProvider sp) =>
{
    var api = sp.GetService<Repo361Api>() ?? throw new ArgumentNullException();
    var mysql = sp.GetService<Repo361MySQLService>() ?? throw new ArgumentNullException();
    await foreach (var file in api.DownloadAsync(new DateTime(2023, 8, 15)))
    {
        await mysql.ImportAsync(file);
    }
    return "OK";
});

app.Run();
