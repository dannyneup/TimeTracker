using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracker.UI.Shared.Models.Employee;
using TimeTracker.UI.Shared.Models.Project;

namespace TimeTracker.UI.Shared.Interfaces.Repositories;

public interface IProjectRepository : IRepository<ProjectRequestModel, ProjectResponseModel>
{
    public Task<(IEnumerable<EmployeeResponseModel>, bool)> GetAssociatedEmployees(int projectId);
}