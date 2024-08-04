using API.Entities;

namespace API.Interfaces;

public interface IFamilyRepository
{
    void AddFamily(Family family);
    public Task<Family> GetFamilyByIdAsync(int familyId);
    public Task<bool> IsFamilyMember(int familyId, string username);
    public Task<bool> IsFamilyMember(int familyId, int userId);
}