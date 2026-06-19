using FinAccountMongoApi.Dtos;
using FinAccountMongoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinAccountMongoApi.Helpers;

public static class ApiResponseHelper
{
    public static ActionResult<ApiResponse<T>> FromFailedOperation<T>(
        ControllerBase controller,
        OperationResult<T> result
    )
    {
        ApiResponse<T> response = ApiResponse<T>.Fail(result.Message);

        if (result.Message.Contains("was not found", StringComparison.OrdinalIgnoreCase))
        {
            return controller.NotFound(response);
        }

        if (result.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
        {
            return controller.Conflict(response);
        }

        return controller.BadRequest(response);
    }
}
