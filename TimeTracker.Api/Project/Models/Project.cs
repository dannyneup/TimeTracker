using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.Api.Project;

[Table("Project")]
public class Project : IEquatable<Project>
{
    [Key]
    public int Id { get; set; }

    [Required] 
    public string Name { get; set; } = null!;
    [Required] 
    public string Customer { get; set; } = null!;
    
    public ICollection<Employee.Models.Employee> Employees { get; set; } = new List<Employee.Models.Employee>();

    public bool Equals(Project? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && Name == other.Name && Customer == other.Customer && Employees.SequenceEqual(other.Employees);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Project)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Customer, Employees);
    }
}