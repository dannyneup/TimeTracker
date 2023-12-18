using System.Net;
using System.Net.Http.Json;
using TimeTracker.Api.Booking.ViewModels;

namespace TimeTracker.Api.Tests.Booking;


public class IntegrationsTestses(TimeTrackerWebApplicationFactory<Program> factory) : BaseIntegrationTests(factory)
{
    private const string BookingEndpoint = "bookings";

    [Theory]
    [InlineData(1, 1, "2023-01-01T15:00-01:00", "2023-01-01T15:15-01:00")]
    public async Task CreateBookingWithValidData(int projectId, int employeeId, string startString, string endString)
    {
        var start = DateTimeOffset.Parse(startString);
        var end = DateTimeOffset.Parse(endString);

        var bookingWriteViewModel = new BookingWriteViewModel(projectId, employeeId, start, end);

        var response = await Client.PostAsJsonAsync(BookingEndpoint, bookingWriteViewModel, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var resultReadViewModel = await response.Content.ReadFromJsonAsync<BookingReadViewModel>();
        Assert.NotNull(resultReadViewModel);

        var resultWriteViewModel = Mapper.Map<BookingWriteViewModel>(resultReadViewModel);

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
        var insertedBooking = await InsertBookingIncludingRequirements();

        var response = await Client.GetAsync($"{BookingEndpoint}/{insertedBooking.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultReadViewModel = await response.Content.ReadFromJsonAsync<BookingReadViewModel>();
        Assert.NotNull(resultReadViewModel);

        var result = Mapper.Map<Api.Booking.Booking>(resultReadViewModel);

        Assert.Equal(insertedBooking, result);
    }


    [Theory]
    [InlineData(1, 1, "2023-01-01T01:00-01:00", "2023-01-01T05:00-01:00")]
    public async Task UpdateExistingBooking(int projectId, int employeeId, string startString, string endString)
    {
        var insertedBooking = await InsertBookingIncludingRequirements();

        var start = DateTimeOffset.Parse(startString);
        var end = DateTimeOffset.Parse(endString);

        var updatedBookingWriteViewModel = new BookingWriteViewModel(projectId, employeeId, start, end);

        var response = await Client.PutAsJsonAsync($"{BookingEndpoint}/{insertedBooking.Id}",
            updatedBookingWriteViewModel, JsonSerializerOptions);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteExistingBooking()
    {
        var insertedBooking = await InsertBookingIncludingRequirements();

        var response = await Client.DeleteAsync($"{BookingEndpoint}/{insertedBooking.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await using var context = Factory.CreateDbContext();
        Assert.NotNull(context.Bookings);

        var foundBooking = await context.Bookings.FindAsync(insertedBooking.Id);
        var bookingRemoved = foundBooking == null;
        Assert.True(bookingRemoved);
    }
}