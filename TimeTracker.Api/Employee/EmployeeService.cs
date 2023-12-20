using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Employee.Models;
using TimeTracker.Api.Repositories;

namespace TimeTracker.Api.Employee;

public class EmployeeService
{
    private readonly IRepository<Models.Employee, EmployeeWriteModel, EmployeeReadModel> _repository;

    public EmployeeService(IRepository<Models.Employee, EmployeeWriteModel, EmployeeReadModel> repository)
    {
        _repository = repository;
    }

    public Task<EmployeeReadModel> CreateEmployeeAsync(EmployeeWriteModel employeeWrite)
    {
        return _repository.AddAsync(employeeWrite);
    }

    public Task<List<EmployeeReadModel>> GetAllEmployeesAsync()
    {
        return _repository.GetAll().ToListAsync();
    }

    public Task<EmployeeReadModel?> GetEmployeeByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task EditEmployee(int id, EmployeeWriteModel employeeResponse)
    {
        return _repository.UpdateAsync(id, employeeResponse);
    }

    public Task DeleteEmployee(int id)
    {
        return _repository.DeleteAsync(id);
    }
}