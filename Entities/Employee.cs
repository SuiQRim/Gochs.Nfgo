using Gochs.Nfgo.Enums;

namespace Gochs.Nfgo.Entities;

public class Employee
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public string PersonnelNumber { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Position { get; set; } = null!;
    public string? Phone { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
    public DateTime CreatedAt { get; set; }

    public Unit Unit { get; set; } = null!;
}
