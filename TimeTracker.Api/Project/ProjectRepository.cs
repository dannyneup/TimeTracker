using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Context;
using TimeTracker.Api.Employee.Models;
using TimeTracker.Api.Project.Models;
using TimeTracker.Api.Repositories;

namespace TimeTracker.Api.Project;

public class ProjectRepository : Repository<Models.Project, ProjectWriteModel, ProjectReadModel>
{
    private readonly IRepository<Employee.Models.Employee, EmployeeWriteModel, EmployeeReadModel> _employeeRepository;

    public ProjectRepository(TimeTrackerContext context, IMapper mapper, IRepository<Employee.Models.Employee, EmployeeWriteModel, EmployeeReadModel> employeeRepository) : base(context, mapper)
    {
        _employeeRepository = employeeRepository;
    }

    public override IQueryable<ProjectReadModel> GetAll()
    {
        var projects = Context.Projects
            .Include(p => p.Employees);
        return projects.ProjectTo<ProjectReadModel>(Mapper.ConfigurationProvider);
    }

    public override async Task<ProjectReadModel?> GetByIdAsync(int id)
    {
        var project = await Context.Projects
            .Include(p => p.Employees)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
        return Mapper.Map<ProjectReadModel>(project);
    }

    public override async Task<ProjectReadModel> AddAsync(ProjectWriteModel write)
    {
        var project = Mapper.Map<Models.Project>(write);

        var employeeIds = write.EmployeeIds;
        List<EmployeeReadModel> employeeReadModels = [];
        foreach (var employeeId in employeeIds)
        {
            var employee = await Context.Employees.FindAsync(employeeId);
            var employeeReadModel = Mapper.Map<EmployeeReadModel>(employee);
            if (employee == null) continue;
            project.Employees.Add(employee);
            employeeReadModels.Add(employeeReadModel);
        }

        Context.Projects.Add(project);
        await Context.SaveChangesAsync();
        
        var response = Mapper.Map<ProjectReadModel>(project) with{Employees = employeeReadModels};
        return response;
    }
    
    public override async Task UpdateAsync(int id, ProjectWriteModel write)
    {
        var project = await Context.Projects.FindAsync(id);
        if(project == null) return;

        var employees = Context.Employees
            .Where(e => write.EmployeeIds.Contains(e.Id))
            .ToList();
        
        project.Employees = employees;
        
        Context.Entry(project).CurrentValues.SetValues(write);
        
        await Context.SaveChangesAsync();
    }
}