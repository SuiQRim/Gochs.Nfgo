using Gochs.Nfgo.DTOs.Units;
using Gochs.Nfgo.Entities;
using Gochs.Nfgo.Exceptions;
using Gochs.Nfgo.Mappings;
using Gochs.Nfgo.Repositories.Interfaces;
using Gochs.Nfgo.Services.Interfaces;

namespace Gochs.Nfgo.Services.Implementations;

public class UnitService : IUnitService
{
    private readonly IUnitRepository unitRepository;
    private readonly IFormationRepository formationRepository;

    public UnitService(IUnitRepository unitRepository, IFormationRepository formationRepository)
    {
        this.unitRepository = unitRepository;
        this.formationRepository = formationRepository;
    }

    public async Task<IReadOnlyCollection<UnitDto>> GetAllAsync() =>
        (await unitRepository.GetAllAsync()).Select(x => x.ToDto()).ToList();

    public async Task<IReadOnlyCollection<UnitDto>> GetByFormationIdAsync(int formationId)
    {
        await EnsureFormationExistsAsync(formationId);
        return (await unitRepository.GetByFormationIdAsync(formationId)).Select(x => x.ToDto()).ToList();
    }

    public async Task<UnitDto> GetByIdAsync(int id)
    {
        var entity = await unitRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Подразделение с id {id} не найдено.");

        return entity.ToDto();
    }

    public async Task<UnitDto> CreateAsync(CreateUnitDto dto)
    {
        await EnsureFormationExistsAsync(dto.FormationId);

        var entity = new Unit
        {
            FormationId = dto.FormationId,
            Name = RequireText(dto.Name, "Название подразделения"),
            Purpose = Normalize(dto.Purpose),
            LeaderName = Normalize(dto.LeaderName)
        };

        await unitRepository.AddAsync(entity);
        return entity.ToDto();
    }

    public async Task<UnitDto> UpdateAsync(int id, UpdateUnitDto dto)
    {
        var entity = await unitRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Подразделение с id {id} не найдено.");

        await EnsureFormationExistsAsync(dto.FormationId);

        entity.FormationId = dto.FormationId;
        entity.Name = RequireText(dto.Name, "Название подразделения");
        entity.Purpose = Normalize(dto.Purpose);
        entity.LeaderName = Normalize(dto.LeaderName);

        await unitRepository.UpdateAsync(entity);
        return entity.ToDto();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await unitRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Подразделение с id {id} не найдено.");

        if (await unitRepository.HasEmployeesAsync(id))
            throw new BusinessRuleException("Нельзя удалить подразделение, пока в нём есть сотрудники.");

        if (await unitRepository.HasEquipmentAsync(id))
            throw new BusinessRuleException("Нельзя удалить подразделение, пока в нём есть техника или имущество.");

        await unitRepository.DeleteAsync(entity);
    }

    private async Task EnsureFormationExistsAsync(int id)
    {
        if (!await formationRepository.ExistsAsync(id))
            throw new NotFoundException($"Формирование с id {id} не найдено.");
    }

    private static string RequireText(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} не может быть пустым.");

        return value.Trim();
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
