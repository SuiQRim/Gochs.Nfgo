using Gochs.Nfgo.Entities;

namespace Gochs.Nfgo.Repositories.Interfaces;

public interface IUnitRepository
{
    Task<List<Unit>> GetAllAsync();
    Task<List<Unit>> GetByFormationIdAsync(int formationId);
    Task<Unit?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> HasEmployeesAsync(int id);
    Task<bool> HasEquipmentAsync(int id);
    Task AddAsync(Unit entity);
    Task UpdateAsync(Unit entity);
    Task DeleteAsync(Unit entity);
}
