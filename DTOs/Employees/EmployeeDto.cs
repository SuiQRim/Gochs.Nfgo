using Gochs.Nfgo.Enums;

namespace Gochs.Nfgo.DTOs.Employees;

public class EmployeeDto
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public string PersonnelNumber { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Position { get; set; } = null!;
    public string? Phone { get; set; }
    public EmployeeStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
