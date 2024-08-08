using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class InvitationsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public InvitationsController(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    [HttpPost("{username}/{familyId}")]
    public async Task<ActionResult> AddInvitation(string username, int familyId)
    {
        var sourceUserId = User.GetUserId();
        var likedUser = await _uow.UserRepository.GetUserByUsernameAsync(username);
        var sourceUser = await _uow.InvitationsRepository.GetUserWithInvitationsAsync(sourceUserId);

        if (likedUser == null)
            return NotFound();

        if (sourceUser.UserName == username)
            return BadRequest("You cannot invite yourself");

        if (!await _uow.FamilyRepository.IsFamilyMember(familyId, sourceUserId))
            return BadRequest("You are not a member of this family");

        var userLike = await _uow.InvitationsRepository.GetUserInvitationAsync(familyId, likedUser.Id);

        if (userLike != null)
            return BadRequest("You already invited this user");

        userLike = new Invitation
        {
            FamilyId = familyId,
            InviterUserId = sourceUserId,
            InviteeUserId = likedUser.Id
        };

        sourceUser.InvitationsSent.Add(userLike);

        if (await _uow.Complete()) return Ok();

        return BadRequest("Failed to invite the user");
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<InvitationDto>>> GetUserInvitations([FromQuery]InvitationsParams invitationParams)
    {
        invitationParams.UserId = User.GetUserId();
        var invitations = await _uow.InvitationsRepository.GetUserInvitationsAsync(invitationParams);

        Response.AddPaginationHeader(new PaginationHeader(invitations.CurrentPage, invitations.PageSize, invitations.TotalCount, invitations.TotalPages));

        return Ok(invitations);
    }

    [HttpPost("accept/{invitationId}")]
    public async Task<ActionResult> AcceptInvitation(int invitationId)
    {
        var userId = User.GetUserId();

        var user = await _uow.UserRepository.GetUserByIdAsync(userId);

        if (user == null)
            return NotFound();

        var invitation = await _uow.InvitationsRepository.GetInvitationByIdAsync(invitationId);

        if (invitation == null)
            return NotFound();

        if (invitation.InviteeUserId != userId)
            return BadRequest("It is not your invitation");

        if (!await _uow.FamilyRepository.IsFamilyMember(invitation.FamilyId, invitation.InviterUserId)){
            await _uow.InvitationsRepository.DeleteInvitationAsync(invitationId);
            return BadRequest("This user has not longer the authority to invite to this family");
        }

        var family = await _uow.FamilyRepository.GetFamilyByIdAsync(invitation.FamilyId);

        if (family == null)
            return NotFound();

        var userFamily = new AppUserFamily{
            UserId = user.Id,
            User = user,
            FamilyId = family.Id,
            Family = family
        };

        user.UserFamilies.Add(userFamily);
        family.UserFamilies.Add(userFamily); //supposedly not neccessery to add to both but better and clearer
        user.InvitationsReceived.Remove(invitation);
        
        if (await _uow.Complete()) return Ok(_mapper.Map<MemberDto>(user));

        return BadRequest("Failed add user to family");
    }
    
    [HttpPost("delete/{invitationId}")]
    public async Task<ActionResult> DeleteInvitation(int invitationId)
    {
        var userId = User.GetUserId();

        var user = await _uow.UserRepository.GetUserByIdAsync(userId);

        if (user == null)
            return NotFound();

        var invitation = await _uow.InvitationsRepository.GetInvitationByIdAsync(invitationId);

        if (invitation == null)
            return NotFound();

        if (invitation.InviteeUserId != userId && invitation.InviterUserId != userId)
            return BadRequest("It is not your invitation");

        if (await _uow.InvitationsRepository.DeleteInvitationAsync(invitationId))
            return Ok();

        return BadRequest("Failed to delete invitation");
    }
}
