using Zin.Infra.Database.Entities;

namespace Zin.API.DTO.EnrollmentTokens;

public record CreateEnrollmentTokenRequest(string? Lifetime, int? MaxDevices);

public record EnrollmentTokenResponse(Guid Id, string Token, DateTime ExpiresAt, int? MaxDevices)
{
    public static EnrollmentTokenResponse From(EnrollmentToken t) => new(t.Id, t.Token, t.ExpiresAt, t.MaxDevices);
}
