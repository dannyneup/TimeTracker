using System.Net;
using System.Net.Http.Json;
using System.Text;
using TimeTracker.Api.Project.ViewModels;

namespace TimeTracker.Api.Tests.Project;

public class IntegrationsTestses(TimeTrackerWebApplicationFactory<Program> factory) : BaseIntegrationTests(factory)
{
    private const string ProjectEndpoint = "projects";

    [Theory]
    [InlineData("Project-Name", "My Customer")]
    
    public async Task CreateProjectWithValidData(string name, string customer)
    {
        var projectWriteViewModel = new ProjectWriteViewModel(name, customer);
            
        var response = await Client.PostAsJsonAsync(ProjectEndpoint, projectWriteViewModel, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var resultReadViewModel = await response.Content.ReadFromJsonAsync<ProjectReadViewModel>();
        Assert.NotNull(resultReadViewModel);

        var resultWriteViewModel = Mapper.Map<ProjectWriteViewModel>(resultReadViewModel);
        
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

        var response = await Client.PostAsync(ProjectEndpoint, httpContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetProjectById()
    {
        var insertedProject = await InsertTestProject();

        var response = await Client.GetAsync($"{ProjectEndpoint}/{insertedProject.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultReadViewModel = await response.Content.ReadFromJsonAsync<ProjectReadViewModel>();
        Assert.NotNull(resultReadViewModel);

        var result = Mapper.Map<TimeTracker.Api.Project.Project>(resultReadViewModel);

        Assert.Equal(insertedProject, result);
    }

    [Theory]
    [InlineData("Updated Dummy-Name", "Updated Dummy-Customer")]
    public async Task UpdateExistingProject(string name, string customer)
    {
        var insertedProject = await InsertTestProject();

        var updatedProjectWriteViewModel = new ProjectWriteViewModel(name, customer);

        var response = await Client.PutAsJsonAsync($"{ProjectEndpoint}/{insertedProject.Id}", updatedProjectWriteViewModel, JsonSerializerOptions);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Theory]
    [InlineData(1, @"{""Name"": ""Updated Dummy-Name""}")]
    public async Task UpdateExistingProjectWithInvalidData(int id, string json)
    {
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.PutAsync($"{ProjectEndpoint}/{id}", httpContent);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Theory]
    [InlineData(5, @"{""Name"": ""Updated Dummy-Name"", ""Customer"": ""Updated Customer-Name""}")]
    public async Task UpdateNonExistingProject(int id, string json)
    {
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.PutAsync($"{ProjectEndpoint}/{id}", httpContent);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteExistingProject()
    {
        var insertedProject = await InsertTestProject();

        var response = await Client.DeleteAsync($"{ProjectEndpoint}/{insertedProject.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await using var context = Factory.CreateDbContext();
        Assert.NotNull(context.Projects);

        var foundProject = await context.Projects.FindAsync(insertedProject.Id);
        var projectRemoved = foundProject == null;
        Assert.True(projectRemoved);
    }
}