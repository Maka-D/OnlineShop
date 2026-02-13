using Identity.API.Models;

namespace Identity.API.Services;

public interface ITokenService
{
    AuthResponse GenerateToken(User user, IList<string> roles);
}