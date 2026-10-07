using Gochs.Nfgo.DTOs.Equipment;
using Gochs.Nfgo.Entities;
using Gochs.Nfgo.Exceptions;
using Gochs.Nfgo.Mappings;
using Gochs.Nfgo.Repositories.Interfaces;
using Gochs.Nfgo.Services.Interfaces;

namespace Gochs.Nfgo.Services.Implementations;

public class EquipmentService : IEquipmentService
{
    private readonly IEquipmentRepository equipmentRepository;
    private readonly IUnitRepository unitRepository;
    private readonly IFormationRepository formationRepository;

    public EquipmentService(
        IEquipmentRepository equipmentRepository,
        IUnitRepository unitRepository,
        IFormationRepository formationRepository)
    {
        this.equipmentRepository = equipmentRepository;
        this.unitRepository = unitRepository;
        this.formationRepository = formationRepository;
    }

    public async Task<IReadOnlyCollection<EquipmentDto>> GetAllAsync() =>
        (await equipmentRepository.GetAllAsync()).Select(x => x.ToDto()).ToList();

    public async Task<IReadOnlyCollection<EquipmentDto>> GetByUnitIdAsync(int unitId)
    {
        await EnsureUnitExistsAsync(unitId);
        return (await equipmentRepository.GetByUnitIdAsync(unitId)).Select(x => x.ToDto()).ToList();
    }

    public async Task<IReadOnlyCollection<EquipmentDto>> GetByFormationIdAsync(int formationId)
    {
        await EnsureFormationExistsAsync(formationId);
        return (await equipmentRepository.GetByFormationIdAsync(formationId)).Select(x => x.ToDto()).ToList();
    }

    public async Task<EquipmentDto> GetByIdAsync(int id)
    {
        var entity = await equipmentRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Equipment with id {id} was not found.");

        return entity.ToDto();
    }

    public async Task<EquipmentDto> CreateAsync(CreateEquipmentDto dto)
    {
        await EnsureUnitExistsAsync(dto.UnitId);
        var inventoryNumber = Normalize(dto.InventoryNumber);

        if (inventoryNumber is not null &&
            await equipmentRepository.InventoryNumberExistsAsync(inventoryNumber))
        {
            throw new BusinessRuleException($"Inventory number '{inventoryNumber}' is already in use.");
        }

        var entity = new Equipment
        {
            UnitId = dto.UnitId,
            Name = dto.Name.Trim(),
            Type = dto.Type!.Value,
            InventoryNumber = inventoryNumber,
            Quantity = dto.Quantity
        };

        await equipmentRepository.AddAsync(entity);
        return entity.ToDto();
    }

    public async Task<EquipmentDto> UpdateAsync(int id, UpdateEquipmentDto dto)
    {
        var entity = await equipmentRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Equipment with id {id} was not found.");

        await EnsureUnitExistsAsync(dto.UnitId);
        var inventoryNumber = Normalize(dto.InventoryNumber);

        if (inventoryNumber is not null &&
            await equipmentRepository.InventoryNumberExistsAsync(inventoryNumber, id))
        {
            throw new BusinessRuleException($"Inventory number '{inventoryNumber}' is already in use.");
        }

        entity.UnitId = dto.UnitId;
        entity.Name = dto.Name.Trim();
        entity.Type = dto.Type!.Value;
        entity.InventoryNumber = inventoryNumber;
        entity.Quantity = dto.Quantity;
        entity.Condition = dto.Condition!.Value;

        await equipmentRepository.UpdateAsync(entity);
        return entity.ToDto();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await equipmentRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Equipment with id {id} was not found.");

        await equipmentRepository.DeleteAsync(entity);
    }

    private async Task EnsureUnitExistsAsync(int id)
    {
        if (!await unitRepository.ExistsAsync(id))
            throw new NotFoundException($"Unit with id {id} was not found.");
    }

    private async Task EnsureFormationExistsAsync(int id)
    {
        if (!await formationRepository.ExistsAsync(id))
            throw new NotFoundException($"Formation with id {id} was not found.");
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
