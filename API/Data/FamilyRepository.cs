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

        query = familyParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x => x.Name)
        };

        return await PagedList<FamilyDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<FamilyDto>(_mapper.ConfigurationProvider), 
            familyParams.PageNumber, 
            familyParams.PageSize);
    }

    public async Task<FamilyDto> GetFamilyDetailsAsync(int familyId)
    {
        var query = _context.Families.AsQueryable();

        query = query.Where(x => x.Id == familyId);

        query = query.Include(x => x.FamilyPhotos);

        return await query.AsNoTracking().ProjectTo<FamilyDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
    }

    public async Task<Family> GetFamilyByIdAsync(int familyId)
    {
        return await _context.Families.FindAsync(familyId);
    }

    public async Task<Family> GetFamilyWithPhotosByIdAsync(int familyId)
    {
        return await _context.Families
            .Include(p => p.FamilyPhotos)
            .SingleOrDefaultAsync(x => x.Id == familyId);
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