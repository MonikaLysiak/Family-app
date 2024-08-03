using API.Entities;

namespace API.Interfaces;

public interface IFamilyRepository
{
    public Task<bool> IsFamilyMember(int familyId, string username);
    public Task<Family> GetFamilyByIdAsync(int familyId);
}