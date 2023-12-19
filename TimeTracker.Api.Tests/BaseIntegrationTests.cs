using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace TimeTracker.Api.Tests;

public abstract class BaseIntegrationTests : IClassFixture<TimeTrackerWebApplicationFactory<Program>>
{
    protected readonly TimeTrackerWebApplicationFactory<Program> Factory;
    protected readonly HttpClient Client;
    protected readonly IMapper Mapper;
    
    protected readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };


    protected BaseIntegrationTests(TimeTrackerWebApplicationFactory<Program> factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
        Mapper = factory.Services.GetRequiredService<IMapper>();
    }
    
    protected async Task<Api.Employee.Models.Employee> InsertTestEmployee()
    {
        await using var context = Factory.CreateDbContext();

        var employee = new Api.Employee.Models.Employee
        {
            Id = 0,
            FirstName = "Max",
            LastName = "Musterman",
            EmailAddress = "max@mustermann.de"
        };

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        return employee;
    }
    
    protected async Task<Api.Project.Project> InsertTestProject()
    {
        await using var context = Factory.CreateDbContext();

        var project = new Api.Project.Project
        {
            Id = 0,
            Name = "Dummy-Name",
            Customer = "Dummy-Customer"
        };

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        return project;
    }
    
    protected async Task<Api.Booking.Booking> InsertTestBooking(int employeeId, int projectId)
    {
        await using var context = Factory.CreateDbContext();

        var booking = new Api.Booking.Booking
        {
            Id = 0,
            EmployeeId = employeeId,
            ProjectId = projectId,
            Start = DateTimeOffset.Parse("2001-01-01T01:00-01:00"),
            End = DateTimeOffset.Parse("2001-01-01T02:00-01:00")
        };

        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        return booking;
    }
    
    protected async Task<Api.Booking.Booking> InsertBookingIncludingRequirements()
    {
        var insertedEmployee = await InsertTestEmployee();
        var insertedProject = await InsertTestProject();
        var insertedBooking = await InsertTestBooking(insertedEmployee.Id, insertedProject.Id);
        return insertedBooking;
    }
}