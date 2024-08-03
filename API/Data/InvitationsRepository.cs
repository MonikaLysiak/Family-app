using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class InvitationsRepository : IInvitationsRepository
{
    private readonly DataContext _context;
    public InvitationsRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<Invitation> GetUserInvitation(int familyId, int targetUserId, int sourceUserId)
    {
        return await _context.Invitations.FindAsync(familyId, targetUserId, sourceUserId);
    }

    public async Task<PagedList<InvitationDto>> GetUserInvitations(InvitationsParams invitationsParams)
    {
        var users = Enumerable.Empty<object>()
                      .Select(_ => new { user = (AppUser)null, family = (Family)null })
                      .AsQueryable();
        var invitations = _context.Invitations.AsQueryable();

        if (invitationsParams.Predicate == "invitationsRecieved")
        {
            invitations = invitations.Where(invite => invite.InviterUserId == invitationsParams.UserId); // likesParams.UserId -> aktualny urzytkownik
            users = invitations.Select(invite => new {user = invite.InviteeUser, family = invite.Family});
        }

        if (invitationsParams.Predicate == "invitationsSent")
        {
            invitations = invitations.Where(like => like.InviteeUserId == invitationsParams.UserId);
            users = invitations.Select(invite => new {user = invite.InviterUser, family = invite.Family});
        }

        var invitationsInfo = users.Select(inviteInfo => new InvitationDto
        {
            Id = inviteInfo.user.Id,

            UserName = inviteInfo.user.UserName,
            Name = inviteInfo.user.Name,
            Surname = inviteInfo.user.Surname,
            UserPhotoUrl = inviteInfo.user.UserPhotos.FirstOrDefault(x => x.IsMain).Url,

            FamilyName = inviteInfo.family.Name,
            FamilyPhotoUrl = inviteInfo.family.FamilyPhotos.FirstOrDefault(x => x.IsMain).Url,
        });

        return await PagedList<InvitationDto>.CreateAsync(invitationsInfo, invitationsParams.PageNumber, invitationsParams.PageSize);
    }

    public async Task<AppUser> GetUserWithInvitations(int userId)
    {
        return await _context.Users
            .Include(x => x.InvitationsSent)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }
}
