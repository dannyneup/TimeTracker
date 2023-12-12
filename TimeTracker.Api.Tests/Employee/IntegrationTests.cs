using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TimeTracker.Api.Context;
using TimeTracker.Api.Employee.ViewModels;

namespace TimeTracker.Api.Tests.Employee;

public class IntegrationsTests : IClassFixture<TimeTrackerWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IMapper _mapper;
    
    public IntegrationsTests(TimeTrackerWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Api.Employee.Employee, EmployeeWriteViewModel>();
            cfg.CreateMap<EmployeeReadViewModel, Api.Employee.Employee>();
        });
        
        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task CreateEmployeeWithValidData()
    {
        var employee = new Api.Employee.Employee
        {
            Id = 0,
            FirstName = "Max",
            LastName = "Muster"
        };
        var employeeWriteViewModel = _mapper.Map<EmployeeWriteViewModel>(employee);

        var response = await _client.PostAsJsonAsync("employees", employeeWriteViewModel,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var resultReadViewModel = await response.Content.ReadFromJsonAsync<EmployeeReadViewModel>();
        Assert.NotNull(resultReadViewModel);

        var result = _mapper.Map<Api.Employee.Employee>(resultReadViewModel);

        Assert.True(EqualsIgnoringId(employee, result));
    }

    [Fact]
    public async Task GetEmployeeById()
    {
        var context = CreateNewBdContext();
        var insertedEmployee = await InsertTestEmployee(context);

        var response = await _client.GetAsync($"employees/{insertedEmployee.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<Api.Employee.Employee>();
        Assert.NotNull(result);

        Assert.True(EqualsIgnoringId(insertedEmployee, result));
    }
    
    [Fact]
    public async Task DeleteExistingEmployee()
    {
        using var scope = _factory.Server.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TimeTrackerContext>();
        var insertedEmployee = await InsertTestEmployee(context);

        var response = await _client.DeleteAsync($"employees/{insertedEmployee.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.NotNull(context.Employees);
 
        var foundEmployee = await context.Employees.FindAsync(insertedEmployee.Id);
        var employeeRemoved = foundEmployee == null;
        
        Assert.True(employeeRemoved);
    }

    private async Task<Api.Employee.Employee> InsertTestEmployee(TimeTrackerContext context)
    {
        var employee = new Api.Employee.Employee()
        {
            Id = 0,
            FirstName = "Max",
            LastName = "Musterman"
        };

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        return employee;
    }

    private TimeTrackerContext CreateNewBdContext()
    {
        using var scope = _factory.Server.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TimeTrackerContext>();

        return context;
    }
    
    private bool EqualsIgnoringId<T>(T obj1, T obj2)
    {
        return typeof(T)
            .GetProperties()
            .Where(p => p.Name != "Id")
            .All(p => Equals(p.GetValue(obj1), p.GetValue(obj2)));
    }
}