using FinAccountMongoApi.Dtos;
using FinAccountMongoApi.Models;
using FinAccountMongoApi.Repositories;
using FinAccountMongoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinAccountMongoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AccountRepository _accountRepository;
    private readonly MovementRepository _movementRepository;
    private readonly DashboardService _dashboardService;

    public DashboardController(
        AccountRepository accountRepository,
        MovementRepository movementRepository,
        DashboardService dashboardService
    )
    {
        _accountRepository = accountRepository;
        _movementRepository = movementRepository;
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<DashboardResponseDto>>> GetDashboard()
    {
        List<AccountDocument> accounts =
            await _accountRepository.GetRawAccountsAsync();

        List<MovementDocument> movements =
            await _movementRepository.GetRawMovementsAsync();

        DashboardResponseDto dashboard =
            _dashboardService.BuildDashboard(accounts, movements);

        return Ok(ApiResponse<DashboardResponseDto>.Ok(
            dashboard,
            "Dashboard retrieved successfully."
        ));
    }
}
