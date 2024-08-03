using API.Entities;
using API.Interfaces;

namespace API.Data;

public class FamilyRepository : IFamilyRepository
{
    private readonly DataContext _context;
    public FamilyRepository(DataContext context)
    {
        _context = context;
    }

    public Task<Family> GetFamilyByIdAsync(int familyId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsFamilyMember(int familyId, string username)
    {
        throw new NotImplementedException();
    }
}