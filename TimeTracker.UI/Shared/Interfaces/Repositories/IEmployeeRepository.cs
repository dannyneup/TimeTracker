using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracker.UI.Shared.Models.Employee;
using TimeTracker.UI.Shared.Models.Project;

namespace TimeTracker.UI.Shared.Interfaces.Repositories;

public interface IEmployeeRepository : IRepository<EmployeeRequestModel, EmployeeResponseModel>
{
    public Task<(IEnumerable<ProjectResponseModel>, bool)> GetAssociatedProjectsAsync(int employeeId);
}