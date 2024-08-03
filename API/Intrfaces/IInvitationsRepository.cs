using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IInvitationsRepository
{
    Task<Invitation> GetUserInvitation(int familyId, int targetUserId, int sourceUserId);
    Task<AppUser> GetUserWithInvitations(int userId);
    Task<PagedList<InvitationDto>> GetUserInvitations(InvitationsParams likesParams);
}
