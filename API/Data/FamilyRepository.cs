using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class FamilyRepository : IFamilyRepository
{
    private readonly DataContext _context;
    public FamilyRepository(DataContext context)
    {
        _context = context;
    }

    public void AddFamily(Family family)
    {
        _context.Families.Add(family);
    }

    public async Task<Family> GetFamilyByIdAsync(int familyId)
    {
        return await _context.Families.FindAsync(familyId);
    }

    public async Task<bool> IsFamilyMember(int familyId, string username)
    {
        return await _context.AppUsersFamilies
            .AnyAsync(uf => uf.FamilyId == familyId && uf.User.UserName == username);
    }
    
    public async Task<bool> IsFamilyMember(int familyId, int userId)
    {
        return await _context.AppUsersFamilies
            .AnyAsync(uf => uf.FamilyId == familyId && uf.UserId == userId);
    }
}