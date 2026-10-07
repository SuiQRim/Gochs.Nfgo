using Gochs.Nfgo.DTOs.Units;

namespace Gochs.Nfgo.Services.Interfaces;

public interface IUnitService
{
    Task<IReadOnlyCollection<UnitDto>> GetAllAsync();
    Task<IReadOnlyCollection<UnitDto>> GetByFormationIdAsync(int formationId);
    Task<UnitDto> GetByIdAsync(int id);
    Task<UnitDto> CreateAsync(CreateUnitDto dto);
    Task<UnitDto> UpdateAsync(int id, UpdateUnitDto dto);
    Task DeleteAsync(int id);
}
