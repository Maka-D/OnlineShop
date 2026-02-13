namespace Identity.API.Models;

public record AuthResponse(string Token, DateTime Expiration, string Email);
public record RegisterRequest(string Email, string Password, string FirstName, string LastName);
public record LoginRequest(string Email, string Password);