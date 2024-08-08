using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IInvitationsRepository
{
    Task<Invitation> GetUserInvitationAsync(int familyId, int targetUserId);
    Task<AppUser> GetUserWithInvitationsAsync(int userId);
    Task<PagedList<InvitationDto>> GetUserInvitationsAsync(InvitationsParams likesParams);
    Task<Invitation> GetInvitationByIdAsync(int id);
    Task<bool> DeleteInvitationAsync(int invitationId);
}
