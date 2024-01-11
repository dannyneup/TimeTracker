﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using TimeTracker.UI.Windows.Shared.Interfaces.Repositories;
using TimeTracker.UI.Windows.Shared.Models.Employee;
using TimeTracker.UI.Windows.Shared.Models.Project;
using TimeTracker.UI.Windows.Shared.Services;

namespace TimeTracker.UI.Windows.Shared.Repositories;

public class EmployeeApiRepository(EndpointService endpointService)
    : ApiRepository<EmployeeRequestModel, EmployeeResponseModel>(endpointService), IEmployeeRepository
{
    public async Task<(IEnumerable<ProjectResponseModel>, bool)> GetAssociatedProjectsAsync(int employeeId)
    {
        var requestFormatUrl = EndpointService.GetUrl<EmployeeRequestModel, ProjectResponseModel>();
        var requestUrl = string.Format(requestFormatUrl, employeeId);
        try
        {
            var response = await HttpClient.GetFromJsonAsync<List<ProjectResponseModel>>(requestUrl);
            return (response ?? Enumerable.Empty<ProjectResponseModel>(), true);
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return (Enumerable.Empty<ProjectResponseModel>(), false);
        }
    }
}