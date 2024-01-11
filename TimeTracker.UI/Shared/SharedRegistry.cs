using System;
using Lamar;
using TimeTracker.UI.Shared.Interfaces.Repositories;
using TimeTracker.UI.Shared.Models;
using TimeTracker.UI.Shared.Repositories;
using TimeTracker.UI.Shared.Services;
using Uri = System.Uri;

namespace TimeTracker.UI.Shared;

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