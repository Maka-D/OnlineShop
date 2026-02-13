using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Models;

namespace Identity.API.Services;

public class IdentityService :IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;

    public IdentityService(UserManager<User> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<Result<bool>> RegisterAsync(RegisterRequest request)
    {
        var user = new User 
        { 
            UserName = request.Email, 
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName 
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join( ";", result.Errors.Select(e => e.Description));
            return Result<bool>.Failure(errors);
        }

        await _userManager.AddToRoleAsync(user, "User");

        return Result<bool>.Success(true);
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result<AuthResponse>.Failure("Invalid credentials.");
        }

        var roles = await _userManager.GetRolesAsync(user);
      
        var authResponse = _tokenService.GenerateToken(user, roles);

        return Result<AuthResponse>.Success(authResponse);
    }
}