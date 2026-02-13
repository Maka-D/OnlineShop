using System.Security.Claims;

namespace Orders.API;

public static class UserHelper
{
    public static string GetUserId(ClaimsPrincipal user) => user.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                                  ?? throw new UnauthorizedAccessException();
}