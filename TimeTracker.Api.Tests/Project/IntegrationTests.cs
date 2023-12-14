using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TimeTracker.Api.Project.ViewModels;

namespace TimeTracker.Api.Tests.Project;

public class IntegrationsTests : IClassFixture<TimeTrackerWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TimeTrackerWebApplicationFactory<Program> _factory;
    private readonly IMapper _mapper;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    private const string ProjectEndpoint = "projects";
    
    public IntegrationsTests(TimeTrackerWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _mapper = factory.Services.GetRequiredService<IMapper>();
    }

    [Theory]
    [InlineData("Project-Name", "My Customer")]
    
    public async Task CreateProjectWithValidData(string name, string customer)
    {
        var projectWriteViewModel = new ProjectWriteViewModel(name, customer);
            
        var response = await _client.PostAsJsonAsync(ProjectEndpoint, projectWriteViewModel, _jsonSerializerOptions);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var resultReadViewModel = await response.Content.ReadFromJsonAsync<ProjectReadViewModel>();
        Assert.NotNull(resultReadViewModel);

        var resultWriteViewModel = _mapper.Map<ProjectWriteViewModel>(resultReadViewModel);
        
        Assert.Equal(projectWriteViewModel, resultWriteViewModel);
    }

    [Theory]
    [InlineData(@"{""Name"": ""Dummy-Name""}")]
    [InlineData(@"{""Customer"": ""Dummy-Customer""}")]
    [InlineData(@"{""Name"": 9}")]
    [InlineData(@"{[""Name"": ""Dummy-Name"", ""Customer"": ""Dummy-Customer""]}")]
    public async Task CreateProjectWithInValidData(string json)
    {
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(ProjectEndpoint, httpContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetProjectById()
    {
        var insertedProject = await InsertTestProject();

        var response = await _client.GetAsync($"{ProjectEndpoint}/{insertedProject.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultReadViewModel = await response.Content.ReadFromJsonAsync<ProjectReadViewModel>();
        Assert.NotNull(resultReadViewModel);

        var result = _mapper.Map<TimeTracker.Api.Project.Project>(resultReadViewModel);

        Assert.Equal(insertedProject, result);
    }

    [Theory]
    [InlineData("Updated Dummy-Name", "Updated Dummy-Customer")]
    public async Task UpdateExistingProject(string name, string customer)
    {
        var insertedProject = await InsertTestProject();

        var updatedProjectWriteViewModel = new ProjectWriteViewModel(name, customer);

        var response = await _client.PutAsJsonAsync($"{ProjectEndpoint}/{insertedProject.Id}", updatedProjectWriteViewModel, _jsonSerializerOptions);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Theory]
    [InlineData(1, @"{""Name"": ""Updated Dummy-Name""}")]
    public async Task UpdateExistingProjectWithInvalidData(int id, string json)
    {
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"{ProjectEndpoint}/{id}", httpContent);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Theory]
    [InlineData(5, @"{""Name"": ""Updated Dummy-Name"", ""Customer"": ""Updated Customer-Name""}")]
    public async Task UpdateNonExistingProject(int id, string json)
    {
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"{ProjectEndpoint}/{id}", httpContent);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteExistingProject()
    {
        var insertedProject = await InsertTestProject();

        var response = await _client.DeleteAsync($"{ProjectEndpoint}/{insertedProject.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await using var context = _factory.CreateDbContext();
        Assert.NotNull(context.Projects);

        var foundProject = await context.Projects.FindAsync(insertedProject.Id);
        var projectRemoved = foundProject == null;
        Assert.True(projectRemoved);
    }

    private async Task<global::TimeTracker.Api.Project.Project> InsertTestProject()
    {
        await using var context = _factory.CreateDbContext();

        var project = new global::TimeTracker.Api.Project.Project
        {
            Id = 0,
            Name = "Dummy-Name",
            Customer = "Dummy-Customer"
        };

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        return project;
    }
}