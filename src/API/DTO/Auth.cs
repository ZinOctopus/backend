namespace Zin.API.DTO.Auth;

public record SignInRequest(string Email, string Password);
public record SignInResponse(string AccessToken, string RefreshToken);
public record AccountResponse(Guid Id, string Email, string Role, string Name);