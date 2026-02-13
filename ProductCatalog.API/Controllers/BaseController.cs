using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace ProductCatalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController :ControllerBase
{
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result == null) return NotFound();

        if (result.IsSuccess)
        {
            if (result.Data == null || result.Data is bool and true)
                return NoContent();

            return Ok(result.Data);
        }

        return result.Message != null && result.Message.Contains("Not found") 
            ? NotFound(result) 
            : BadRequest(result);
    }
}