using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IFamilyRepository
{
    void AddFamily(Family family);
    Task<PagedList<FamilyDto>> GetUserFamiliesAsync(FamilyParams userParams);
    public Task<Family> GetFamilyByIdAsync(int familyId);
    public Task<bool> IsFamilyMember(int familyId, string username);
    public Task<bool> IsFamilyMember(int familyId, int userId);
}