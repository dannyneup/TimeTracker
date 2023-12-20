using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Context;
using TimeTracker.Api.Employee.Models;
using TimeTracker.Api.Project.Models;
using TimeTracker.Api.Repositories;

namespace TimeTracker.Api.Project;

public class ProjectRepository : Repository<Models.Project, ProjectRequestModel, ProjectResponseModel>
{
    private readonly IRepository<Employee.Models.Employee, EmployeeRequestModel, EmployeeResponseModel> _employeeRepository;

    public ProjectRepository(TimeTrackerContext context, IMapper mapper, IRepository<Employee.Models.Employee, EmployeeRequestModel, EmployeeResponseModel> employeeRepository) : base(context, mapper)
    {
        _employeeRepository = employeeRepository;
    }

    public override async Task<List<ProjectResponseModel>> GetAllAsync()
    {
        var projects = await Context.Projects
            .Include(p => p.Employees)
            .ToListAsync();
        return Mapper.Map<List<ProjectResponseModel>>(projects);
    }

    public override async Task<ProjectResponseModel> AddAsync(ProjectRequestModel request)
    {
        var project = Mapper.Map<Models.Project>(request);
        
        Context.Projects.Add(project);
        await Context.SaveChangesAsync();

        var employeeResponses = (await _employeeRepository.GetAllAsync())
            .Where(e => request.EmployeeIds.Contains(e.Id))
            .ToList();
        
        var response = Mapper.Map<ProjectResponseModel>(project) with{Employees = employeeResponses};
        return response;
    }
    
    public override async Task UpdateAsync(int id, ProjectRequestModel request)
    {
        var project = await Context.Projects.FindAsync(id);
        if(project == null) return;

        var employees = Context.Employees
            .Where(e => request.EmployeeIds.Contains(e.Id))
            .ToList();
        
        project.Employees = employees;
        
        Context.Entry(project).CurrentValues.SetValues(request);
        
        await Context.SaveChangesAsync();
    }
}