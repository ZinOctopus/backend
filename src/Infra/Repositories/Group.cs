using Zin.Infra.Database.Context;
using Zin.Infra.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace Zin.Infra.Repository;

public class GroupRepository(DBContext _context) : BaseRepository<Group>(_context)
{
    public async Task<bool> NameTaken(string name, Guid? exceptId = null)
    {
        string lower = name.ToLower();
        return await _context.Set<Group>().AnyAsync(g => g.Name.ToLower() == lower && g.Id != exceptId);
    }
}
