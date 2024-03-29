using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Booking.Models;
using TimeTracker.Api.Context;
using TimeTracker.Api.Employee;
using TimeTracker.Api.Employee.Models;
using TimeTracker.Api.Project;
using TimeTracker.Api.Project.Models;
using TimeTracker.Api.Repositories;
using TimeTracker.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddDbContext<TimeTrackerContext>(options => options
        .UseSqlite(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IRepository<Employee, EmployeeWriteModel, EmployeeReadModel>,
    EmployeeRepository>();
builder.Services.AddScoped<IRepository<Project, ProjectWriteModel, ProjectReadModel>, 
    ProjectRepository>();
builder.Services.AddScoped<IRepository<Booking, BookingWriteModel, BookingReadModel>,
    Repository<Booking, BookingWriteModel, BookingReadModel>>();

builder.Services.AddScoped<EmailValidationService>();
builder.Services.AddScoped<WorkingHoursCalculationService>();
builder.Services.AddScoped<ObjectPropertyCheckingService>();

builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<EmployeeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

namespace TimeTracker.Api
{
    public abstract class Program;
}