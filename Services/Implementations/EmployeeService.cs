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
            ?? throw new NotFoundException($"Employee with id {id} was not found.");

        return entity.ToDto();
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        await EnsureUnitExistsAsync(dto.UnitId);

        var personnelNumber = RequireText(dto.PersonnelNumber, "Personnel number");
        var fullName = RequireText(dto.FullName, "Employee full name");
        var position = RequireText(dto.Position, "Employee position");

        if (await employeeRepository.PersonnelNumberExistsAsync(personnelNumber))
            throw new BusinessRuleException($"Personnel number '{personnelNumber}' is already in use.");

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
            ?? throw new NotFoundException($"Employee with id {id} was not found.");

        await EnsureUnitExistsAsync(dto.UnitId);

        var personnelNumber = RequireText(dto.PersonnelNumber, "Personnel number");
        var fullName = RequireText(dto.FullName, "Employee full name");
        var position = RequireText(dto.Position, "Employee position");

        if (await employeeRepository.PersonnelNumberExistsAsync(personnelNumber, id))
            throw new BusinessRuleException($"Personnel number '{personnelNumber}' is already in use.");

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
            ?? throw new NotFoundException($"Employee with id {id} was not found.");

        await employeeRepository.DeleteAsync(entity);
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

    private static string RequireText(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} cannot be empty.");

        return value.Trim();
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
