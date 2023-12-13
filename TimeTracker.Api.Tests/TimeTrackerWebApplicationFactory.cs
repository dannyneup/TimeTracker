using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimeTracker.Api.Context;

namespace TimeTracker.Api.Tests;

public class TimeTrackerWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly SqliteConnection _sqLiteConnection = new("Data Source=:memory:");
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _sqLiteConnection.Open();
        
        builder.ConfigureServices(services =>
        {
            
            services.AddDbContext<TimeTrackerContext>(
                options => options.UseSqlite(_sqLiteConnection));
        });

        builder.UseEnvironment("Development");
    }

    public TimeTrackerContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TimeTrackerContext>()
            .UseSqlite(_sqLiteConnection)
            .Options;

        return new TimeTrackerContext(options);
    }
}