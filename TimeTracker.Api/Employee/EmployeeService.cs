using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Booking.Models;
using TimeTracker.Api.Employee.Models;
using TimeTracker.Api.Project.Models;
using TimeTracker.Api.Repositories;
using TimeTracker.Api.Services;

namespace TimeTracker.Api.Employee;

public class EmployeeService
{
    private readonly IRepository<Models.Employee, EmployeeWriteModel, EmployeeReadModel> _repository;
    private readonly IRepository<Project.Models.Project, ProjectWriteModel, ProjectReadModel> _projectRepository;
    private readonly IRepository<Booking.Models.Booking, BookingWriteModel, BookingReadModel> _bookingRepository;
    private readonly WorkingHoursCalculationService _workingHoursCalculationService;

    public EmployeeService(IRepository<Models.Employee, EmployeeWriteModel, EmployeeReadModel> repository,
        IRepository<Project.Models.Project, ProjectWriteModel, ProjectReadModel> projectRepository, 
        IRepository<Booking.Models.Booking, BookingWriteModel, BookingReadModel> bookingRepository,
        WorkingHoursCalculationService workingHoursCalculationService)
    {
        _repository = repository;
        _projectRepository = projectRepository;
        _bookingRepository = bookingRepository;
        _workingHoursCalculationService = workingHoursCalculationService;
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

    public async Task<List<ProjectReadModel>> GetEmployeesProjectsAsync(int id)
    {
        var employeeReadModel = await _repository.GetByIdAsync(id);
        if (employeeReadModel == null) return [];
        var projects = await _projectRepository.GetAll()
            .ToListAsync();
        return projects.Where(p => p.Employees.Contains(employeeReadModel)).ToList();
    }

    public async Task<TimeSpan> GetEmployeesWorkingHoursAsync(int id, DateTimeOffset start, DateTimeOffset end)
    {
        var bookings = await _bookingRepository.GetAll().ToListAsync();
        var filteredBookings = bookings.Where(b => b.EmployeeId == id);
        var workingHours = _workingHoursCalculationService.GetEmployeeWorkingHours(filteredBookings, start, end);
        return workingHours;
    }

    public async Task<TimeSpan> GetEmployeesWorkingHoursDeviationAsync(int id, DateTimeOffset start, DateTimeOffset end)
    {
        var bookings = await _bookingRepository.GetAll().ToListAsync();
        var filteredBookings = bookings.Where(b => b.EmployeeId == id);
        var workingHours = _workingHoursCalculationService.GetEmployeeWorkingHoursDeviation(filteredBookings, start, end);
        return workingHours;
    }
}