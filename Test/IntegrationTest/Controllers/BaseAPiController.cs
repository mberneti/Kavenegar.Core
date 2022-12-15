using Microsoft.AspNetCore.Mvc;

namespace IntegrationTest.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class BaseAPiController : ControllerBase
{
    protected IActionResult HandleValue<T>(
        T? value)
    {
        if (value == null) return NoContent();
        return Ok(value);
    }
}