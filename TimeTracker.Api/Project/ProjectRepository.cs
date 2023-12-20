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

    public override IQueryable<ProjectResponseModel> GetAll()
    {
        var projects = Context.Projects
            .Include(p => p.Employees)
            .AsQueryable();
        return Mapper.Map<IQueryable<ProjectResponseModel>>(projects);
    }

    public override async Task<ProjectResponseModel> AddAsync(ProjectRequestModel request)
    {
        var project = Mapper.Map<Models.Project>(request);
        
        Context.Projects.Add(project);
        await Context.SaveChangesAsync();

        var employeeIds = request.EmployeeIds;
        List<EmployeeResponseModel> employeeResponses = [];
        foreach (var employeeId in employeeIds)
        {
            var employeeResponse = await _employeeRepository.GetByIdAsync(employeeId);
            if (employeeResponse == null) continue;
            employeeResponses.Add(employeeResponse);
        }
        
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