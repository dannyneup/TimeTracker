using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TimeTracker.Api.Booking.ViewModels;

namespace TimeTracker.Api.Tests.Booking;

[Collection("sequential")]
public class IntegrationsTests : IClassFixture<TimeTrackerWebApplicationFactory<Program>>
{
    private const string BookingEndpoint = "bookings";
    private readonly HttpClient _client;
    private readonly TimeTrackerWebApplicationFactory<Program> _factory;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly IMapper _mapper;

    public IntegrationsTests(TimeTrackerWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _mapper = factory.Services.GetRequiredService<IMapper>();
    }

    [Theory]
    [InlineData(1, 1, "2023-01-01T15:00-01:00", "2023-01-01T15:15-01:00")]
    public async Task CreateBookingWithValidData(int projectId, int employeeId, string startString, string endString)
    {
        var start = DateTimeOffset.Parse(startString);
        var end = DateTimeOffset.Parse(endString);

        var bookingWriteViewModel = new BookingWriteViewModel(projectId, employeeId, start, end);

        var response = await _client.PostAsJsonAsync(BookingEndpoint, bookingWriteViewModel, _jsonSerializerOptions);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var resultReadViewModel = await response.Content.ReadFromJsonAsync<BookingReadViewModel>();
        Assert.NotNull(resultReadViewModel);

        var resultWriteViewModel = _mapper.Map<BookingWriteViewModel>(resultReadViewModel);

        Assert.Equal(bookingWriteViewModel, resultWriteViewModel);
    }
    
    /*
    [Theory]
    [InlineData(@"{""ProjectId"": 1, ""EmployeeId"": 1, ""Start"": ""2023-01-01T15:00-01:00"", ""End"": ""2023-01-01T15:00-01:00""}")]
    public async Task CreateBookingWithInValidData(string json)
    {
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(BookingEndpoint, httpContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    */
    
    [Fact]
    public async Task GetBookingById()
    {
        var insertedBooking = await InsertTestBooking();

        var response = await _client.GetAsync($"{BookingEndpoint}/{insertedBooking.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultReadViewModel = await response.Content.ReadFromJsonAsync<BookingReadViewModel>();
        Assert.NotNull(resultReadViewModel);

        var result = _mapper.Map<Api.Booking.Booking>(resultReadViewModel);

        Assert.Equal(insertedBooking, result);
    }


    [Theory]
    [InlineData(1, 1, "2023-01-01T01:00-01:00", "2023-01-01T05:00-01:00")]
    public async Task UpdateExistingBooking(int projectId, int employeeId, string startString, string endString)
    {
        var insertedBooking = await InsertTestBooking();

        var start = DateTimeOffset.Parse(startString);
        var end = DateTimeOffset.Parse(endString);

        var updatedBookingWriteViewModel = new BookingWriteViewModel(projectId, employeeId, start, end);

        var response = await _client.PutAsJsonAsync($"{BookingEndpoint}/{insertedBooking.Id}",
            updatedBookingWriteViewModel, _jsonSerializerOptions);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteExistingBooking()
    {
        var insertedBooking = await InsertTestBooking();

        var response = await _client.DeleteAsync($"{BookingEndpoint}/{insertedBooking.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await using var context = _factory.CreateDbContext();
        Assert.NotNull(context.Bookings);

        var foundBooking = await context.Bookings.FindAsync(insertedBooking.Id);
        var bookingRemoved = foundBooking == null;
        Assert.True(bookingRemoved);
    }

    private async Task<Api.Booking.Booking> InsertTestBooking()
    {
        await using var context = _factory.CreateDbContext();

        var employee = new Api.Employee.Employee
        {
            Id = 0,
            LastName = "Muster",
            FirstName = "Max",
            EmailAddress = "max@mustermann.de"
        };
        context.Employees.Add(employee);

        var project = new Api.Project.Project
        {
            Id = 0,
            Name = "Dummy-Project",
            Customer = "Dummy-Customer",
            Employees = [employee]
        };
        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var booking = new Api.Booking.Booking
        {
            Id = 0,
            EmployeeId = employee.Id,
            ProjectId = project.Id,
            Start = DateTimeOffset.Parse("2001-01-01T01:00-01:00"),
            End = DateTimeOffset.Parse("2001-01-01T02:00-01:00")
        };

        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        return booking;
    }
}