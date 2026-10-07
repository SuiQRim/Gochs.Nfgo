using Gochs.Nfgo.Entities;

namespace Gochs.Nfgo.Repositories.Interfaces;

public interface IFormationRepository
{
    Task<List<Formation>> GetAllAsync();
    Task<Formation?> GetByIdAsync(int id);
    Task<Formation?> GetDetailsAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> HasUnitsAsync(int id);
    Task<bool> HasNotificationsAsync(int id);
    Task AddAsync(Formation entity);
    Task UpdateAsync(Formation entity);
    Task DeleteAsync(Formation entity);
}
