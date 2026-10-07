using Gochs.Nfgo.DTOs.Employees;
using Gochs.Nfgo.Entities;
using Gochs.Nfgo.Exceptions;
using Gochs.Nfgo.Mappings;
using Gochs.Nfgo.Repositories.Interfaces;
using Gochs.Nfgo.Services.Interfaces;

namespace Gochs.Nfgo.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository employeeRepository;
    private readonly IUnitRepository unitRepository;
    private readonly IFormationRepository formationRepository;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IUnitRepository unitRepository,
        IFormationRepository formationRepository)
    {
        this.employeeRepository = employeeRepository;
        this.unitRepository = unitRepository;
        this.formationRepository = formationRepository;
    }

    public async Task<IReadOnlyCollection<EmployeeDto>> GetAllAsync() =>
        (await employeeRepository.GetAllAsync()).Select(x => x.ToDto()).ToList();

    public async Task<IReadOnlyCollection<EmployeeDto>> GetByUnitIdAsync(int unitId)
    {
        await EnsureUnitExistsAsync(unitId);
        return (await employeeRepository.GetByUnitIdAsync(unitId)).Select(x => x.ToDto()).ToList();
    }

    public async Task<IReadOnlyCollection<EmployeeDto>> GetByFormationIdAsync(int formationId)
    {
        await EnsureFormationExistsAsync(formationId);
        return (await employeeRepository.GetByFormationIdAsync(formationId)).Select(x => x.ToDto()).ToList();
    }

    public async Task<EmployeeDto> GetByIdAsync(int id)
    {
        var entity = await employeeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Сотрудник с id {id} не найден.");

        return entity.ToDto();
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        await EnsureUnitExistsAsync(dto.UnitId);

        var personnelNumber = RequireText(dto.PersonnelNumber, "Табельный номер");
        var fullName = RequireText(dto.FullName, "ФИО сотрудника");
        var position = RequireText(dto.Position, "Должность сотрудника");

        if (await employeeRepository.PersonnelNumberExistsAsync(personnelNumber))
            throw new BusinessRuleException($"Табельный номер '{personnelNumber}' уже используется.");

        var entity = new Employee
        {
            UnitId = dto.UnitId,
            PersonnelNumber = personnelNumber,
            FullName = fullName,
            Position = position,
            Phone = Normalize(dto.Phone)
        };

        await employeeRepository.AddAsync(entity);
        return entity.ToDto();
    }

    public async Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var entity = await employeeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Сотрудник с id {id} не найден.");

        await EnsureUnitExistsAsync(dto.UnitId);

        var personnelNumber = RequireText(dto.PersonnelNumber, "Табельный номер");
        var fullName = RequireText(dto.FullName, "ФИО сотрудника");
        var position = RequireText(dto.Position, "Должность сотрудника");

        if (await employeeRepository.PersonnelNumberExistsAsync(personnelNumber, id))
            throw new BusinessRuleException($"Табельный номер '{personnelNumber}' уже используется.");

        entity.UnitId = dto.UnitId;
        entity.PersonnelNumber = personnelNumber;
        entity.FullName = fullName;
        entity.Position = position;
        entity.Phone = Normalize(dto.Phone);
        entity.Status = dto.Status!.Value;

        await employeeRepository.UpdateAsync(entity);
        return entity.ToDto();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await employeeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Сотрудник с id {id} не найден.");

        await employeeRepository.DeleteAsync(entity);
    }

    private async Task EnsureUnitExistsAsync(int id)
    {
        if (!await unitRepository.ExistsAsync(id))
            throw new NotFoundException($"Подразделение с id {id} не найдено.");
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
