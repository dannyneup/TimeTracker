using System;
using AutoMapper;
using Lamar;
using TimeTracker.UI.Windows.Pages.EmployeeOverviewPage.Records;
using TimeTracker.UI.Windows.Shared.Interfaces.Repositories;
using TimeTracker.UI.Windows.Shared.Models;
using TimeTracker.UI.Windows.Shared.Models.Employee;
using TimeTracker.UI.Windows.Shared.Repositories;
using TimeTracker.UI.Windows.Shared.Services;
using Uri = System.Uri;

namespace TimeTracker.UI.Windows.Shared;

public class SharedRegistry : ServiceRegistry
{
    public SharedRegistry()
    {
        For<IEmployeeRepository>()
            .Use<EmployeeApiRepository>().Singleton();
        For<IProjectRepository>()
            .Use<ProjectApiRepository>().Singleton();

        For<EndpointService>().Use(new EndpointService(CreateBaseUrl())).Singleton();

        For<ApplicationModel>().Use(new ApplicationModel()).Singleton();

    }

    private static Uri CreateBaseUrl()
    {
        var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
        return environment == "Development" ? new Uri("http://localhost:5021") : new Uri("http://0.0.0.0:0");
    }
}