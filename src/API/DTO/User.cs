using System.ComponentModel.DataAnnotations;

using Zin.Infra.Database.Entities;
using Zin.Infra.Repository;

namespace Zin.API.DTO.Users;

public static class AccountRules
{
    public const int MaxName = 100;
    public const int MaxEmail = 255;
    public const int MinPassword = 8;

    public static string? NameError(string name) =>
        name.Length == 0 ? "name is required" : name.Length > MaxName ? $"name must be at most {MaxName} characters" : null;

    public static string? EmailError(string email) =>
        email.Length == 0 || email.Length > MaxEmail || !new EmailAddressAttribute().IsValid(email) ? "email is not valid" : null;

    public static string? PasswordError(string? password) =>
        password == null || password.Length < MinPassword ? $"password must be at least {MinPassword} characters" : null;
}

public record CreateUserRequest(string? Name, string? Email, string? Password, string? Role);

public record UserAccess(Guid[]? GroupIds, Guid[]? DeviceIds)
{
    public static UserAccess From(UserAccessIds? ids) => new(ids?.GroupIds ?? [], ids?.DeviceIds ?? []);
}

public record UpdateUserRequest(string? Name, bool? Disabled, string? Password, UserAccess? Access);

public record UpdateAccountRequest(string? Name, string? CurrentPassword, string? Password);

public record UserResponse(Guid Id, string Email, string Name, string Role, string LastSignIn, bool Disabled, UserAccess Access)
{
    public static UserResponse From(User u, UserAccessIds? access) =>
        new(u.Id, u.Email, u.Name, u.Role, u.LastSignIn?.ToString("O") ?? "", u.Disabled, UserAccess.From(access));
}
