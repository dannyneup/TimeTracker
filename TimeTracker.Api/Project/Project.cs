using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.Api.Project;

[Table("Project")]
public class Project
{
    [Key]
    public int Id { get; set; }

    [Required] 
    public string Name { get; set; } = null!;
    [Required] 
    public string Customer { get; set; } = null!;

    public ICollection<Employee.Employee>? Employees { get; set; } = new List<Employee.Employee>();
}