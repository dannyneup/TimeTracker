using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracker.UI.Windows.Shared.Interfaces.Repositories;
using TimeTracker.UI.Windows.Shared.Models.Employee;
using TimeTracker.UI.Windows.Shared.Models.Project;
using TimeTracker.UI.Windows.Shared.Services;

namespace TimeTracker.UI.Windows.Shared.Repositories;

public class ProjectApiRepository(EndpointService endpointService)
    : ApiRepository<ProjectRequestModel, ProjectResponseModel>(endpointService), IProjectRepository
{
    public async Task<(IEnumerable<EmployeeResponseModel>, bool)> GetAssociatedEmployees(int projectId)
    {
        var requestFormatUrl = EndpointService.GetUrl<ProjectRequestModel, EmployeeResponseModel>();
        var requestUrl = string.Format(requestFormatUrl, projectId);
        return await GetAllAsync<EmployeeResponseModel>(requestUrl);
    }
}