using Gochs.Nfgo.Entities;

namespace Gochs.Nfgo.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();
    Task<List<Employee>> GetByUnitIdAsync(int unitId);
    Task<List<Employee>> GetByFormationIdAsync(int formationId);
    Task<Employee?> GetByIdAsync(int id);
    Task<bool> PersonnelNumberExistsAsync(string personnelNumber, int? excludeId = null);
    Task AddAsync(Employee entity);
    Task UpdateAsync(Employee entity);
    Task DeleteAsync(Employee entity);
}
