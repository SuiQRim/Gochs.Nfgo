using Gochs.Nfgo.DTOs.Dashboard;
using Gochs.Nfgo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gochs.Nfgo.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        this.dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardDto>> Get()
    {
        return Ok(await dashboardService.GetAsync());
    }
}
