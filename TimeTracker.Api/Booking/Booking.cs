using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.Api.Booking;

[Table("Booking")]
public class Booking : IEquatable<Booking>
{
    [Key]
    public required int Id { get; set; }
    [Required]
    [ForeignKey("Employee")]
    public required int EmployeeId { get; set; }
    [Required]
    [ForeignKey("Project")] 
    public required int ProjectId { get; set; }
    
    [Required]
    public required DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }

    public Employee.Models.Employee Employee { get; set; } = null!;
    public Project.Project Project { get; set; } = null!;

    public bool Equals(Booking? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && EmployeeId == other.EmployeeId && ProjectId == other.ProjectId && Start.Equals(other.Start) && End.Equals(other.End);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Booking)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, EmployeeId, ProjectId, Start, End);
    }
}