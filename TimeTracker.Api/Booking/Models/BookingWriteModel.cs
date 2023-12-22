namespace TimeTracker.Api.Booking.Models;

public record BookingWriteModel(int ProjectId, int EmployeeId, DateTimeOffset Start, DateTimeOffset End);
