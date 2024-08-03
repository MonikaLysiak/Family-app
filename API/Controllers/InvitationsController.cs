using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class InvitationsController : BaseApiController
{
    private readonly IUnitOfWork _uow;

    public InvitationsController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddInvitation(string username, int familyId)
    {
        var sourceUserId = User.GetUserId();
        var likedUser = await _uow.UserRepository.GetUserByUsernameAsync(username);
        var sourceUser = await _uow.LikesRepository.GetUserWithInvitations(sourceUserId);

        if (likedUser == null)
            return NotFound();

        if (sourceUser.UserName == username)
            return BadRequest("You cannot invite yourself");

        if (!await _uow.FamilyRepository.IsFamilyMember(familyId, username))
            return BadRequest("You are not a member of this family");

        var userLike = await _uow.LikesRepository.GetUserInvitation(familyId, likedUser.Id, sourceUserId);

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
    public async Task<ActionResult<PagedList<InvitationDto>>> GetUserLikes([FromQuery]InvitationsParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
        var users = await _uow.LikesRepository.GetUserInvitations(likesParams);

        Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

        return Ok(users);
    }

}
