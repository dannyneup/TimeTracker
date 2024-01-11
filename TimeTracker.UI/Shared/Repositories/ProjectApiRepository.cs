using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracker.UI.Shared.Interfaces.Repositories;
using TimeTracker.UI.Shared.Models.Employee;
using TimeTracker.UI.Shared.Models.Project;
using TimeTracker.UI.Shared.Services;

namespace TimeTracker.UI.Shared.Repositories;

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