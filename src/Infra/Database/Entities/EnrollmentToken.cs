namespace Zin.Infra.Database.Entities;

public class EnrollmentToken : IBase
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public int? MaxDevices { get; set; }
}
