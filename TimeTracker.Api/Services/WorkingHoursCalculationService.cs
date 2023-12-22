using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Booking.Models;

namespace TimeTracker.Api.Services;

public class WorkingHoursCalculationService
{
    private const int DailyPlannedHours = 8;

    public TimeSpan GetEmployeeWorkingHours(IEnumerable<BookingReadModel> bookings, DateTimeOffset start, DateTimeOffset end)
    {
        var filteredBookings = bookings.Where(b => b.Start >= start && b.End <= end);
        var totalWorkingMinutes = filteredBookings.Sum(b => (b.End - b.Start).TotalMinutes);
        var totalWorkingHours = TimeSpan.FromMinutes(totalWorkingMinutes);

        return totalWorkingHours;
    }

    public TimeSpan GetEmployeeWorkingHoursDeviation(IEnumerable<BookingReadModel> bookings, DateTimeOffset start, DateTimeOffset end)
    {
        var totalWorkingHours = GetEmployeeWorkingHours(bookings, start, end);
        var timespan = end - start;
        var timespanDays = timespan.Days + 1;
        var plannedHours = timespanDays * DailyPlannedHours;
        var deviation = totalWorkingHours - TimeSpan.FromHours(plannedHours);
        return deviation;
    }
}