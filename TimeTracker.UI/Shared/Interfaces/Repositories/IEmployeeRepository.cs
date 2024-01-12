using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracker.UI.Shared.Models.Employee;
using TimeTracker.UI.Shared.Models.Project;

namespace TimeTracker.UI.Shared.Interfaces.Repositories;

public interface IEmployeeRepository : IRepository<EmployeeRequestModel, EmployeeResponseModel>
{
    public Task<(IEnumerable<ProjectResponseModel>, bool)> GetAssociatedProjectsAsync(int employeeId);
    public Task<(TimeSpan, bool)> GetTotalWorkingHoursAsync(int employeeId, DateTimeOffset start, DateTimeOffset end);
    public Task<(TimeSpan, bool)> GetTodaysTotalWorkingHoursAsync(int employeeId);
    public Task<(IDictionary<int, TimeSpan>, bool)> GetProjectWorkingHoursAsync(int employeeId, DateTimeOffset start, DateTimeOffset end);
    public Task<(IDictionary<int, TimeSpan>, bool)> GetTodaysProjectWorkingHoursAsync(int employeeId);
}