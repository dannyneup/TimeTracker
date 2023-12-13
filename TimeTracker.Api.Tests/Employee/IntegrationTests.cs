using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TimeTracker.Api.Employee.ViewModels;

namespace TimeTracker.Api.Tests.Employee;

public class IntegrationsTests : IClassFixture<TimeTrackerWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TimeTrackerWebApplicationFactory<Program> _factory;
    private readonly IMapper _mapper;

    public IntegrationsTests(TimeTrackerWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _mapper = factory.Services.GetRequiredService<IMapper>();
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

    [Theory]
    [InlineData(@"{""id"": 20, ""FirstName"": ""Max"", ""LastName"": ""Mustermann""}")]
    [InlineData(@"{""FirstName"": ""Max""}")]
    [InlineData(@"{""LastName"": ""Mustermann""}")]
    [InlineData(@"{}")]
    [InlineData(@"{""FirstName"": 3, ""LastName"": ""Mustermann""}")]
    [InlineData(@"{""FirstName"": ""Max"", ""LastName"": [Mustermann]}")]
    [InlineData(@"{[{""FirstName"": ""Max"", ""LastName"": ""Mustermann""}]}")]
    [InlineData(@"{[{""FirstName"": ""Max"", ""LastName"": ""Mustermann""}, {""FirstName"": ""Max"", ""LastName"": ""Mustermann""}]}")]
public async Task CreateEmployeeWithInValidData(string json)
    {
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("employees", httpContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetEmployeeById()
    {
        var insertedEmployee = await InsertTestEmployee();

        var response = await _client.GetAsync($"employees/{insertedEmployee.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultReadViewModel = await response.Content.ReadFromJsonAsync<EmployeeReadViewModel>();
        Assert.NotNull(resultReadViewModel);

        var result = _mapper.Map<Api.Employee.Employee>(resultReadViewModel);

        Assert.Equal(insertedEmployee, result);
    }
    
    [Fact]
    public async Task DeleteExistingEmployee()
    {
        var insertedEmployee = await InsertTestEmployee();

        var response = await _client.DeleteAsync($"employees/{insertedEmployee.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await using var context = _factory.CreateDbContext();
        Assert.NotNull(context.Employees);
 
        var foundEmployee = await context.Employees.FindAsync(insertedEmployee.Id);
        var employeeRemoved = foundEmployee == null;
        Assert.True(employeeRemoved);
    }

    private async Task<Api.Employee.Employee> InsertTestEmployee()
    {
        await using var context = _factory.CreateDbContext();
        
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

    private bool EqualsIgnoringId<T>(T obj1, T obj2)
    {
        return typeof(T)
            .GetProperties()
            .Where(p => p.Name != "Id")
            .All(p => Equals(p.GetValue(obj1), p.GetValue(obj2)));
    }
}