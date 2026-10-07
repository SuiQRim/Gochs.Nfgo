using Gochs.Nfgo.DTOs.Dashboard;
using Gochs.Nfgo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.Nfgo.Controllers;

/// <summary>
/// Сводные показатели состояния НФГО.
/// </summary>
[ApiController]
[Route("api/dashboard")]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        this.dashboardService = dashboardService;
    }

    /// <summary>
    /// Получить показатели Dashboard.
    /// </summary>
    /// <response code="200">Сводные показатели успешно рассчитаны.</response>
    [HttpGet]
    [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardDto>> Get()
    {
        return Ok(await dashboardService.GetAsync());
    }
}
