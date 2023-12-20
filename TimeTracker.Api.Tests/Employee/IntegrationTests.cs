using System.Net;
using System.Net.Http.Json;
using System.Text;
using TimeTracker.Api.Employee.ViewModels;

namespace TimeTracker.Api.Tests.Employee;

public class IntegrationsTests(TimeTrackerWebApplicationFactory<Program> factory) : BaseIntegrationTests(factory)
{
    private const string EmployeeEndpoint = "employees";

    [Theory]
    [InlineData("Mustermann", "Max", "max@mustermann.de")]
    public async Task CreateEmployeeWithValidData(string lastName, string firstName, string emailAddress)
    {
        var employeeWriteViewModel = new EmployeeWriteViewModel(lastName, firstName, emailAddress);
            
        var response = await Client.PostAsJsonAsync(EmployeeEndpoint, employeeWriteViewModel, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var resultReadViewModel = await response.Content.ReadFromJsonAsync<EmployeeReadViewModel>();
        Assert.NotNull(resultReadViewModel);

        var resultWriteViewModel = Mapper.Map<EmployeeWriteViewModel>(resultReadViewModel);
        
        Assert.Equal(employeeWriteViewModel, resultWriteViewModel);
    }

    [Theory]
    [InlineData(@"{""FirstName"": ""Max""}")]
    [InlineData(@"{""LastName"": ""Mustermann""}")]
    [InlineData(@"{}")]
    [InlineData(@"{""FirstName"": 3, ""LastName"": ""Mustermann""}")]
    [InlineData(@"{""FirstName"": ""Max"", ""LastName"": [Mustermann]}")]
    [InlineData(@"{[{""FirstName"": ""Max"", ""LastName"": ""Mustermann""}]}")]
    [InlineData(@"{[{""FirstName"": ""Max"", ""LastName"": ""Mustermann""}, {""FirstName"": ""Max"", ""LastName"": ""Mustermann""}]}")]
    [InlineData(@"{""FirstName"": ""Max"", ""LastName"": ""Mustermann"", ""EmailAdress"": ""Maxmustermann.de""}")]
    [InlineData(@"{""FirstName"": ""Max"", ""LastName"": ""Mustermann"", ""EmailAdress"": ""Max@mustermann""}")]
    public async Task CreateEmployeeWithInValidData(string json)
    {
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.PostAsync(EmployeeEndpoint, httpContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetEmployeeById()
    {
        var insertedEmployee = await InsertTestEmployee();

        var response = await Client.GetAsync($"{EmployeeEndpoint}/{insertedEmployee.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultReadViewModel = await response.Content.ReadFromJsonAsync<EmployeeReadViewModel>();
        Assert.NotNull(resultReadViewModel);

        var result = Mapper.Map<Api.Employee.Models.Employee>(resultReadViewModel);

        Assert.Equal(insertedEmployee, result);
    }

    [Theory]
    [InlineData("Musterfrau", "Erika", "erika@musterfrau.de")]
    public async Task UpdateExistingEmployee(string lastName, string firstName, string emailAddress)
    {
        var insertedEmployee = await InsertTestEmployee();

        var updatedEmployeeWriteViewModel = new EmployeeWriteViewModel(lastName, firstName, emailAddress);

        var response = await Client.PutAsJsonAsync($"{EmployeeEndpoint}/{insertedEmployee.Id}", updatedEmployeeWriteViewModel, JsonSerializerOptions);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Theory]
    [InlineData(2, @"{""LastName"": ""Musterfrau""}")]
    [InlineData(1, @"{""LastName"": ""Musterfrau"", ""FirstName"": ""Erika"", ""EmailAddress"": ""erikamusterfrau.de""}")]
    public async Task UpdateExistingEmployeeWithInvalidData(int id, string json)
    {
        await InsertTestEmployee();
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.PutAsync($"{EmployeeEndpoint}/{id}", httpContent);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Theory]
    [InlineData(5, @"{""LastName"": ""Musterfrau"", ""FirstName"": ""Erika"", ""EmailAddress"": ""erika@musterfrau.de""}")]
    public async Task UpdateNonExistingEmployee(int id, string json)
    {
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.PutAsync($"{EmployeeEndpoint}/{id}", httpContent);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteExistingEmployee()
    {
        var insertedEmployee = await InsertTestEmployee();

        var response = await Client.DeleteAsync($"{EmployeeEndpoint}/{insertedEmployee.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await using var context = Factory.CreateDbContext();
        Assert.NotNull(context.Employees);

        var foundEmployee = await context.Employees.FindAsync(insertedEmployee.Id);
        var employeeRemoved = foundEmployee == null;
        Assert.True(employeeRemoved);
    }
}