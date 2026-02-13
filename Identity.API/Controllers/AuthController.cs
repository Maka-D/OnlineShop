using Identity.API.Models;
using Identity.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

public class AuthController :BaseController
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("user/register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        return HandleResult(await _identityService.RegisterAsync(request));
    }

    [HttpPost("user/login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        return HandleResult(await _identityService.LoginAsync(request));
    }
}