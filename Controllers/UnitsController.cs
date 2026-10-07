using Gochs.Nfgo.DTOs.Common;
using Gochs.Nfgo.DTOs.Units;
using Gochs.Nfgo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.Nfgo.Controllers;

/// <summary>
/// Управление подразделениями формирований НФГО.
/// </summary>
[ApiController]
[Route("api/units")]
[Produces("application/json")]
public class UnitsController : ControllerBase
{
    private readonly IUnitService unitService;

    public UnitsController(IUnitService unitService)
    {
        this.unitService = unitService;
    }

    /// <summary>
    /// Получить список всех подразделений.
    /// </summary>
    /// <response code="200">Список подразделений успешно получен.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UnitDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<UnitDto>>> GetAll()
    {
        return Ok(await unitService.GetAllAsync());
    }

    /// <summary>
    /// Получить подразделение по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор подразделения.</param>
    /// <response code="200">Подразделение найдено.</response>
    /// <response code="404">Подразделение не найдено.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UnitDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UnitDto>> GetById(int id)
    {
        return Ok(await unitService.GetByIdAsync(id));
    }

    /// <summary>
    /// Получить подразделения конкретного формирования.
    /// </summary>
    /// <param name="formationId">Идентификатор формирования.</param>
    /// <response code="200">Список подразделений успешно получен.</response>
    /// <response code="404">Формирование не найдено.</response>
    [HttpGet("/api/formations/{formationId:int}/units")]
    [ProducesResponseType(typeof(IEnumerable<UnitDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyCollection<UnitDto>>> GetByFormation(int formationId)
    {
        return Ok(await unitService.GetByFormationIdAsync(formationId));
    }

    /// <summary>
    /// Создать подразделение.
    /// </summary>
    /// <param name="dto">Данные нового подразделения.</param>
    /// <response code="201">Подразделение создано.</response>
    /// <response code="400">Переданы некорректные данные.</response>
    /// <response code="404">Указанное формирование не найдено.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UnitDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UnitDto>> Create(CreateUnitDto dto)
    {
        var result = await unitService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновить подразделение.
    /// </summary>
    /// <param name="id">Идентификатор подразделения.</param>
    /// <param name="dto">Новые данные подразделения.</param>
    /// <response code="200">Подразделение обновлено.</response>
    /// <response code="400">Переданы некорректные данные.</response>
    /// <response code="404">Подразделение или указанное формирование не найдено.</response>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(UnitDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UnitDto>> Update(int id, UpdateUnitDto dto)
    {
        return Ok(await unitService.UpdateAsync(id, dto));
    }

    /// <summary>
    /// Удалить подразделение.
    /// </summary>
    /// <param name="id">Идентификатор подразделения.</param>
    /// <response code="204">Подразделение удалено.</response>
    /// <response code="404">Подразделение не найдено.</response>
    /// <response code="409">Удаление запрещено, пока в подразделении есть сотрудники или техника.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        await unitService.DeleteAsync(id);
        return NoContent();
    }
}
