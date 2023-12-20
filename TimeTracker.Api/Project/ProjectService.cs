using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Project.Models;
using TimeTracker.Api.Repositories;

namespace TimeTracker.Api.Project;

public class ProjectService
{
    private readonly IRepository<Models.Project, ProjectWriteModel, ProjectReadModel> _repository;

    public ProjectService(IRepository<Models.Project, ProjectWriteModel, ProjectReadModel> repository)
    {
        _repository = repository;
    }

    public Task<ProjectReadModel> CreateProjectAsync(ProjectWriteModel projectWrite)
    {
        return _repository.AddAsync(projectWrite);
    }

    public Task<List<ProjectReadModel>> GetAllProjectsAsync()
    {
        return _repository.GetAll().ToListAsync();
    }

    public Task<ProjectReadModel?> GetProjectByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task EditProject(int id, ProjectWriteModel projectResponse)
    {
        return _repository.UpdateAsync(id, projectResponse);
    }

    public Task DeleteProject(int id)
    {
        return _repository.DeleteAsync(id);
    }
}