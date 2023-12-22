using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Context;
using TimeTracker.Api.Employee.Models;
using TimeTracker.Api.Repositories;

namespace TimeTracker.Api.Employee;

public class EmployeeRepository : Repository<Employee.Models.Employee, EmployeeWriteModel, EmployeeReadModel>
{
    public EmployeeRepository(TimeTrackerContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public override IQueryable<EmployeeReadModel> GetAll()
    {
        var employees = Context.Employees
            .Include(e => e.Projects)
            .Include(e => e.Bookings);
        return employees.ProjectTo<EmployeeReadModel>(Mapper.ConfigurationProvider);
    }

    public override async Task<EmployeeReadModel?> GetByIdAsync(int id)
    {
        var employee = await Context.Employees
            .Include(e => e.Projects)
            .Include(e => e.Bookings)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
        return Mapper.Map<EmployeeReadModel>(employee);
    }
    
    public override async Task UpdateAsync(int id, EmployeeWriteModel write)
    {
        var employee = await Context.Employees.FindAsync(id);
        if(employee == null) return;
        
        Context.Entry(employee).CurrentValues.SetValues(write);
        
        await Context.SaveChangesAsync();
    }
}