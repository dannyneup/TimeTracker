using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracker.UI.Windows.Shared.Models.Employee;
using TimeTracker.UI.Windows.Shared.Models.Project;

namespace TimeTracker.UI.Windows.Shared.Interfaces.Repositories;

public interface IEmployeeRepository : IRepository<EmployeeRequestModel, EmployeeResponseModel>
{
    public Task<(IEnumerable<ProjectResponseModel>, bool)> GetAssociatedProjectsAsync(int employeeId);
}