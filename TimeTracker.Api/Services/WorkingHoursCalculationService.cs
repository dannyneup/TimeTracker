using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Context;

namespace TimeTracker.Api.Services;

public class WorkingHoursCalculationService
{
    private readonly TimeTrackerContext _context;
    private const int DailyPlannedHours = 8;

    public WorkingHoursCalculationService(TimeTrackerContext context)
    {
        _context = context;
    }

    public async Task<TimeSpan> GetEmployeeWorkingHours(int employeeId, DateTimeOffset start, DateTimeOffset end)
    {
        var bookings = await _context.Bookings
            .Where(b => b.EmployeeId == employeeId)
            .ToListAsync();
        var totalWorkingMinutes = bookings.Sum(b => (b.End - b.Start).TotalMinutes);
        var totalWorkingHours = TimeSpan.FromMinutes(totalWorkingMinutes);

        return totalWorkingHours;
    }

    public async Task<TimeSpan> GetEmployeeWorkingHoursDeviation(int employeeId, DateTimeOffset start, DateTimeOffset end)
    {
        var totalWorkingHours = await GetEmployeeWorkingHours(employeeId, start, end);
        var timespan = end - start;
        var timespanDays = timespan.Days + 1;
        var plannedHours = timespanDays * DailyPlannedHours;
        var deviation = totalWorkingHours - TimeSpan.FromHours(plannedHours);
        return deviation;
    }
}