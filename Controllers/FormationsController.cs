using Gochs.Nfgo.DTOs.Common;
using Gochs.Nfgo.DTOs.Formations;
using Gochs.Nfgo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.Nfgo.Controllers;

/// <summary>
/// Управление формированиями НФГО.
/// </summary>
[ApiController]
[Route("api/formations")]
[Produces("application/json")]
public class FormationsController : ControllerBase
{
    private readonly IFormationService formationService;

    public FormationsController(IFormationService formationService)
    {
        this.formationService = formationService;
    }

    /// <summary>
    /// Получить список всех формирований НФГО.
    /// </summary>
    /// <response code="200">Список формирований успешно получен.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FormationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<FormationDto>>> GetAll()
    {
        return Ok(await formationService.GetAllAsync());
    }

    /// <summary>
    /// Получить подробную карточку формирования.
    /// </summary>
    /// <param name="id">Идентификатор формирования.</param>
    /// <response code="200">Формирование найдено.</response>
    /// <response code="404">Формирование не найдено.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FormationDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FormationDetailsDto>> GetById(int id)
    {
        return Ok(await formationService.GetByIdAsync(id));
    }

    /// <summary>
    /// Создать формирование НФГО.
    /// </summary>
    /// <param name="dto">Данные нового формирования.</param>
    /// <response code="201">Формирование создано.</response>
    /// <response code="400">Переданы некорректные данные.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(FormationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FormationDto>> Create(CreateFormationDto dto)
    {
        var result = await formationService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Обновить формирование НФГО.
    /// </summary>
    /// <param name="id">Идентификатор формирования.</param>
    /// <param name="dto">Новые данные формирования.</param>
    /// <response code="200">Формирование обновлено.</response>
    /// <response code="400">Переданы некорректные данные.</response>
    /// <response code="404">Формирование не найдено.</response>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(FormationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FormationDto>> Update(int id, UpdateFormationDto dto)
    {
        return Ok(await formationService.UpdateAsync(id, dto));
    }

    /// <summary>
    /// Удалить формирование НФГО.
    /// </summary>
    /// <param name="id">Идентификатор формирования.</param>
    /// <response code="204">Формирование удалено.</response>
    /// <response code="404">Формирование не найдено.</response>
    /// <response code="409">Удаление запрещено из-за связанных подразделений или истории оповещений.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        await formationService.DeleteAsync(id);
        return NoContent();
    }
}
