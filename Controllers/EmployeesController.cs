using Gochs.Nfgo.DTOs.Employees;
using Gochs.Nfgo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.Nfgo.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetAll()
    {
        return Ok(await employeeService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        return Ok(await employeeService.GetByIdAsync(id));
    }

    [HttpGet("/api/units/{unitId:int}/employees")]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetByUnit(int unitId)
    {
        return Ok(await employeeService.GetByUnitIdAsync(unitId));
    }

    [HttpGet("/api/formations/{formationId:int}/employees")]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetByFormation(int formationId)
    {
        return Ok(await employeeService.GetByFormationIdAsync(formationId));
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
    {
        var result = await employeeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> Update(int id, UpdateEmployeeDto dto)
    {
        return Ok(await employeeService.UpdateAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await employeeService.DeleteAsync(id);
        return NoContent();
    }
}
