using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using TimeTracker.UI.Windows.Pages.EmployeeOverviewPage.Records;
using TimeTracker.UI.Windows.Pages.ProjectOverviewPage.Records;
using TimeTracker.UI.Windows.Shared.Interfaces.Repositories;
using TimeTracker.UI.Windows.Shared.Models.Employee;
using TimeTracker.UI.Windows.Shared.Models.Project;
using TimeTracker.UI.Windows.Shared.Services;

namespace TimeTracker.UI.Windows.Shared.Repositories;

public class ProjectApiRepository(EndpointService endpointService)
    : ApiRepository<Project, ProjectRequestModel, ProjectResponseModel>(endpointService), IProjectRepository
{
    public async Task<(IEnumerable<Employee>, bool)> GetAssociatedEmployees(int projectId)
    {
        var requestFormatUrl = EndpointService.GetUrl<ProjectRequestModel, EmployeeResponseModel>();
        var requestUrl = string.Format(requestFormatUrl, projectId);
        try
        {
            var response = await HttpClient.GetFromJsonAsync<List<ProjectResponseModel>>(requestUrl);
            return (Mapper.Map<IEnumerable<Employee>>(response) ?? Enumerable.Empty<Employee>(), true);
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return (Enumerable.Empty<Employee>(), false);
        }
    }
}