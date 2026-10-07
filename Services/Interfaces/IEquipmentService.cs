using Gochs.Nfgo.DTOs.Equipment;

namespace Gochs.Nfgo.Services.Interfaces;

public interface IEquipmentService
{
    Task<IReadOnlyCollection<EquipmentDto>> GetAllAsync();
    Task<IReadOnlyCollection<EquipmentDto>> GetByUnitIdAsync(int unitId);
    Task<IReadOnlyCollection<EquipmentDto>> GetByFormationIdAsync(int formationId);
    Task<EquipmentDto> GetByIdAsync(int id);
    Task<EquipmentDto> CreateAsync(CreateEquipmentDto dto);
    Task<EquipmentDto> UpdateAsync(int id, UpdateEquipmentDto dto);
    Task DeleteAsync(int id);
}
