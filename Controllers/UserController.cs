using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ArquiteturaBaseApi.DTOs;
using ArquiteturaBaseApi.Services.Interfaces;

namespace ArquiteturaBaseApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userService.CreateAsync(request, cancellationToken);
        return Created(string.Empty, user);
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetPagedAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }
}
