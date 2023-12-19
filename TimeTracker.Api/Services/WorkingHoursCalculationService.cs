using System.Runtime.InteropServices.JavaScript;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Context;

namespace TimeTracker.Api.Services;

public class WorkingHoursCalculationService
{
    private readonly TimeTrackerContext _context;
    private const int WeeklyPlannedHours = 40;

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
}