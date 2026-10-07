using Gochs.Nfgo.Entities;

namespace Gochs.Nfgo.Repositories.Interfaces;

public interface IEquipmentRepository
{
    Task<List<Equipment>> GetAllAsync();
    Task<List<Equipment>> GetByUnitIdAsync(int unitId);
    Task<List<Equipment>> GetByFormationIdAsync(int formationId);
    Task<Equipment?> GetByIdAsync(int id);
    Task<bool> InventoryNumberExistsAsync(string inventoryNumber, int? excludeId = null);
    Task AddAsync(Equipment entity);
    Task UpdateAsync(Equipment entity);
    Task DeleteAsync(Equipment entity);
}
