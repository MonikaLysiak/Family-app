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

    public async Task<Invitation> GetUserInvitationAsync(int familyId, int targetUserId)
    {
        return await _context.Invitations.SingleOrDefaultAsync(i => i.FamilyId == familyId && i.InviteeUserId == targetUserId);
    }

    public async Task<PagedList<InvitationDto>> GetUserInvitationsAsync(InvitationsParams invitationsParams)
    {
        var users = Enumerable.Empty<object>()
                      .Select(_ => new { id = -1, user = (AppUser)null, family = (Family)null })
                      .AsQueryable();
        var invitations = _context.Invitations.AsQueryable();

        if (invitationsParams.Predicate == "sent")
        {
            invitations = invitations.Where(invite => invite.InviterUserId == invitationsParams.UserId);
            users = invitations.Select(invite => new {id = invite.Id, user = invite.InviteeUser, family = invite.Family});
        }

        if (invitationsParams.Predicate == "received")
        {
            invitations = invitations.Where(like => like.InviteeUserId == invitationsParams.UserId);
            users = invitations.Select(invite => new {id = invite.Id, user = invite.InviterUser, family = invite.Family});
        }

        var invitationsInfo = users.Select(inviteInfo => new InvitationDto
        {
            Id = inviteInfo.id,

            UserName = inviteInfo.user.UserName,
            Name = inviteInfo.user.Name,
            Surname = inviteInfo.user.Surname,
            UserPhotoUrl = inviteInfo.user.UserPhotos.FirstOrDefault(x => x.IsMain).Url,

            FamilyName = inviteInfo.family.Name,
            FamilyPhotoUrl = inviteInfo.family.FamilyPhotos.FirstOrDefault(x => x.IsMain).Url,
        });

        return await PagedList<InvitationDto>.CreateAsync(invitationsInfo, invitationsParams.PageNumber, invitationsParams.PageSize);
    }

    public async Task<AppUser> GetUserWithInvitationsAsync(int userId)
    {
        return await _context.Users
            .Include(x => x.InvitationsSent)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<Invitation> GetInvitationByIdAsync(int invitationId)
    {
        return await _context.Invitations.FindAsync(invitationId);
    }

    public async Task<bool> DeleteInvitationAsync(int invitationId)
    {
        var invitation = await _context.Invitations.FindAsync(invitationId);

        if (invitation == null)
        {
            return false;
        }

        _context.Invitations.Remove(invitation);

        return await _context.SaveChangesAsync() > 0;
    }
}
