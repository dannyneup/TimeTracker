namespace TimeTracker.Api.Booking.Models;

public record BookingReadModel(int Id, int ProjectId, int EmployeeId, DateTimeOffset Start, DateTimeOffset End);
