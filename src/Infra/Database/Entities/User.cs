namespace Zin.Infra.Database.Entities;

public static class Roles
{
    public const string System = "System";
    public const string User = "User";
}

public class User : IBase
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool Disabled { get; set; } = false;
    public DateTime? LastSignIn { get; set; } = null;
    public string Role { get; set; } = Roles.User;
}