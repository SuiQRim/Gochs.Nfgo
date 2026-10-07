using Gochs.Nfgo.DTOs.Employees;

namespace Gochs.Nfgo.Services.Interfaces;

public interface IEmployeeService
{
    Task<IReadOnlyCollection<EmployeeDto>> GetAllAsync();
    Task<IReadOnlyCollection<EmployeeDto>> GetByUnitIdAsync(int unitId);
    Task<IReadOnlyCollection<EmployeeDto>> GetByFormationIdAsync(int formationId);
    Task<EmployeeDto> GetByIdAsync(int id);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
    Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto dto);
    Task DeleteAsync(int id);
}
