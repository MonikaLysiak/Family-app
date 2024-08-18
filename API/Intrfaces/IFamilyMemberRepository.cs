
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IFamilyMemberRepository
{
    public Task<string> GetFamilyMemberNicknameAsync(int familyId, int userId);
    public Task<PagedList<MemberDto>> GetFamilyMembersAsync(FamilyMemberParams familyMemberParams);
    Task<AppUserFamily> GetAppUserFamilyAsync(int familyId, int userId);
}