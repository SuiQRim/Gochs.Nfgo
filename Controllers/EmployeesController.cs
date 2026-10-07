using Gochs.Nfgo.DTOs.Common;
using Gochs.Nfgo.DTOs.Employees;
using Gochs.Nfgo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.Nfgo.Controllers;

/// <summary>
/// Управление личным составом НФГО.
/// </summary>
[ApiController]
[Route("api/employees")]
[Produces("application/json")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }

    /// <summary>
    /// Получить весь личный состав.
    /// </summary>
    /// <response code="200">Список сотрудников успешно получен.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetAll()
    {
        return Ok(await employeeService.GetAllAsync());
    }

    /// <summary>
    /// Получить сотрудника по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <response code="200">Сотрудник найден.</response>
    /// <response code="404">Сотрудник не найден.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        return Ok(await employeeService.GetByIdAsync(id));
    }

    /// <summary>
    /// Получить сотрудников конкретного подразделения.
    /// </summary>
    /// <param name="unitId">Идентификатор подразделения.</param>
    /// <response code="200">Список сотрудников успешно получен.</response>
    /// <response code="404">Подразделение не найдено.</response>
    [HttpGet("/api/units/{unitId:int}/employees")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetByUnit(int unitId)
    {
        return Ok(await employeeService.GetByUnitIdAsync(unitId));
    }

    /// <summary>
    /// Получить сотрудников конкретного формирования.
    /// </summary>
    /// <param name="formationId">Идентификатор формирования.</param>
    /// <response code="200">Список сотрудников успешно получен.</response>
    /// <response code="404">Формирование не найдено.</response>
    [HttpGet("/api/formations/{formationId:int}/employees")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyCollection<EmployeeDto>>> GetByFormation(int formationId)
    {
        return Ok(await employeeService.GetByFormationIdAsync(formationId));
    }

    /// <summary>
    /// Добавить сотрудника в личный состав НФГО.
    /// </summary>
    /// <param name="dto">Данные сотрудника.</param>
    /// <response code="201">Сотрудник создан.</response>
    /// <response code="400">Переданы некорректные данные.</response>
    /// <response code="404">Указанное подразделение не найдено.</response>
    /// <response code="409">Табельный номер уже используется.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
    {
        var result = await employeeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновить данные сотрудника.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <param name="dto">Новые данные сотрудника.</param>
    /// <response code="200">Сотрудник обновлён.</response>
    /// <response code="400">Переданы некорректные данные.</response>
    /// <response code="404">Сотрудник или указанное подразделение не найдено.</response>
    /// <response code="409">Табельный номер уже используется.</response>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EmployeeDto>> Update(int id, UpdateEmployeeDto dto)
    {
        return Ok(await employeeService.UpdateAsync(id, dto));
    }

    /// <summary>
    /// Удалить сотрудника.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <response code="204">Сотрудник удалён.</response>
    /// <response code="404">Сотрудник не найден.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await employeeService.DeleteAsync(id);
        return NoContent();
    }
}
