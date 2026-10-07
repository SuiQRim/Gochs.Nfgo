using Gochs.Nfgo.DTOs.Dashboard;

namespace Gochs.Nfgo.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetAsync();
}
