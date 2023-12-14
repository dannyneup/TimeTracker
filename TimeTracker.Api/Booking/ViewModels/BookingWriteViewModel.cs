namespace TimeTracker.Api.Booking.ViewModels;

public record BookingWriteViewModel(int ProjectId, int EmployeeId, DateTimeOffset Start, DateTimeOffset End);