using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ArquiteturaBaseApi.Services.Interfaces;

namespace ArquiteturaBaseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloWorldController : ControllerBase
{
    private readonly IMongoTestService _mongoTestService;

    public HelloWorldController(IMongoTestService mongoTestService)
    {
        _mongoTestService = mongoTestService;
    }

    [HttpGet("publico")]
    public IActionResult GetPublic()
    {
        return Ok(new { message = "Hello World" });
    }

    [Authorize]
    [HttpGet("privado")]
    public async Task<IActionResult> GetPrivate(CancellationToken cancellationToken)
    {
        var dbStatus = await _mongoTestService.TestConnectionAsync(cancellationToken);
        return Ok(new { 
            message = "Hello World Autenticado!",
            mongoStatus = dbStatus
        });
    }
}
