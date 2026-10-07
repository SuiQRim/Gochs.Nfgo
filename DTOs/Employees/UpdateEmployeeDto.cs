using System.ComponentModel.DataAnnotations;
using Gochs.Nfgo.Enums;

namespace Gochs.Nfgo.DTOs.Employees;

public class UpdateEmployeeDto
{
    [Range(1, int.MaxValue)]
    public int UnitId { get; set; }

    [Required]
    [StringLength(50)]
    public string PersonnelNumber { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Position { get; set; } = null!;

    [StringLength(30)]
    public string? Phone { get; set; }

    [Required]
    public EmployeeStatus? Status { get; set; }
}
