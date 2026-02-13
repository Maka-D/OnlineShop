using Identity.API.Models;
using Shared.Models;

namespace Identity.API.Services;

public interface IIdentityService
{
    Task<Result<bool>> RegisterAsync(RegisterRequest request);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
}