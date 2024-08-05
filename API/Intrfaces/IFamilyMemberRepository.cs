
using API.DTOs;
using API.Helpers;

namespace API.Interfaces;

public interface IFamilyMemberRepository
{
    public Task<string> GetFamilyMemberNickname(int familyId, int userId);
    public Task<PagedList<MemberDto>> GetFamilyMembersAsync(FamilyMemberParams familyMemberParams);
}