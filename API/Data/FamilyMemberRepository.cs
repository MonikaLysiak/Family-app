using API.DTOs;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class FamilyMemberRepository : IFamilyMemberRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public FamilyMemberRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PagedList<MemberDto>> GetFamilyMembersAsync(FamilyMemberParams familyMemberParams)
    {
        var test = _context.AppUsersFamilies.AsQueryable();

        test = test.Where(uf => uf.FamilyId == familyMemberParams.FamilyId);

        test = test.Include(uf => uf.User);

        test = test.Include(uf => uf.User.UserPhotos);

        test = familyMemberParams.OrderBy switch
        {
            "created" => test.OrderByDescending(x => x.User.Created),
            _ => test.OrderByDescending(x => x.User.Name)
        };

        return await PagedList<MemberDto>.CreateAsync(
            test.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider), 
            familyMemberParams.PageNumber, 
            familyMemberParams.PageSize);
    }

    public async Task<string> GetFamilyMemberNickname(int familyId, int userId)
    {
        return (await _context.AppUsersFamilies.FindAsync(familyId, userId)).Nickname;
    }
}