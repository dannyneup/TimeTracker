namespace TimeTracker.Api.Booking.ViewModels;

public record BookingReadViewModel(int Id, int ProjectId, int EmployeeId, DateTimeOffset Start, DateTimeOffset End);