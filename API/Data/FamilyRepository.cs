using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class FamilyRepository : IFamilyRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public FamilyRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void AddFamily(Family family)
    {
        _context.Families.Add(family);
    }

    public async Task<PagedList<FamilyDto>> GetUserFamiliesAsync(FamilyParams familyParams)
    {
        var query = _context.Families.AsQueryable();

        query = query.Where(x => x.UserFamilies.Any(uf => uf.UserId == familyParams.CurrentUserId));

        //add family created and fix below
        query = familyParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Name),
            _ => query.OrderByDescending(x => x.Name)
        };

        return await PagedList<FamilyDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<FamilyDto>(_mapper.ConfigurationProvider), 
            familyParams.PageNumber, 
            familyParams.PageSize);
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