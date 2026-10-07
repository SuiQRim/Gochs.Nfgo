using Gochs.Nfgo.Data;
using Gochs.Nfgo.Entities;
using Gochs.Nfgo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gochs.Nfgo.Repositories.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext context;

    public EmployeeRepository(AppDbContext context)
    {
        this.context = context;
    }

    public Task<List<Employee>> GetAllAsync() =>
        context.Employees.AsNoTracking().OrderBy(x => x.FullName).ToListAsync();

    public Task<List<Employee>> GetByUnitIdAsync(int unitId) =>
        context.Employees.AsNoTracking()
            .Where(x => x.UnitId == unitId)
            .OrderBy(x => x.FullName)
            .ToListAsync();

    public Task<List<Employee>> GetByFormationIdAsync(int formationId) =>
        context.Employees.AsNoTracking()
            .Where(x => x.Unit.FormationId == formationId)
            .OrderBy(x => x.FullName)
            .ToListAsync();

    public Task<Employee?> GetByIdAsync(int id) =>
        context.Employees.FirstOrDefaultAsync(x => x.Id == id);

    public Task<bool> PersonnelNumberExistsAsync(string personnelNumber, int? excludeId = null) =>
        context.Employees.AnyAsync(x =>
            x.PersonnelNumber == personnelNumber &&
            (!excludeId.HasValue || x.Id != excludeId.Value));

    public async Task AddAsync(Employee entity)
    {
        context.Employees.Add(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee entity)
    {
        context.Employees.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Employee entity)
    {
        context.Employees.Remove(entity);
        await context.SaveChangesAsync();
    }
}
