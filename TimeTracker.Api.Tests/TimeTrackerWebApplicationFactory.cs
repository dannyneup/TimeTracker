using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TimeTracker.Api.Context;

namespace TimeTracker.Api.Tests;

public class TimeTrackerWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private SqliteConnection _sqLiteConnection = new ("Data Source=:memory:");
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<TimeTrackerContext>));
            services.AddDbContext<TimeTrackerContext>(
                options => options.UseSqlite(_sqLiteConnection));
        });
        
        _sqLiteConnection.Open();

        using var context = CreateDbContext();
        context.Database.EnsureCreated();

        builder.UseEnvironment("Development");
    }
    
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _sqLiteConnection?.Close();
            _sqLiteConnection?.Dispose();
        }
    }

    public TimeTrackerContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TimeTrackerContext>()
            .UseSqlite(_sqLiteConnection)
            .Options;

        return new TimeTrackerContext(options);
    }
}