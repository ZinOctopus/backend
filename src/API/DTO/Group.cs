using Zin.Infra.Database.Entities;

namespace Zin.API.DTO.Groups;

public record GroupRequest(string? Name);

public record GroupResponse(Guid Id, string Name)
{
    public static GroupResponse From(Group g) => new(g.Id, g.Name);
}
