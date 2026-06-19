using FinAccountMongoApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FinAccountMongoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<object>> GetHealth()
    {
        var health = new
        {
            status = "Healthy",
            service = "finaccount-mongo-api",
            checkedAtUtc = DateTime.UtcNow
        };

        return Ok(ApiResponse<object>.Ok(
            health,
            "API is healthy."
        ));
    }
}
