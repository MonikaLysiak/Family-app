using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API;

public class MessagesController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public MessagesController(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var userId = User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(createMessageDto.FamilyId, userId))
            return BadRequest("You are not a member of this family");

        var sender = await _uow.UserRepository.GetUserByIdAsync(userId);
        var family = await _uow.FamilyRepository.GetFamilyByIdAsync(createMessageDto.FamilyId);

        if (family == null) return NotFound("There is no family of that id");

        var message = new Message
        {
            Sender = sender,
            Family = family,
            SenderId = sender.Id,
            SenderUsername = sender.UserName,
            FamilyId = family.Id,
            Content = createMessageDto.Content
        };

        _uow.MessageRepository.AddMessage(message);

        if (await _uow.Complete()) return Ok(_mapper.Map<MessageDto>(message));

        return BadRequest("Failed to send message");
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForFamily([FromQuery] MessageParams messageParams)
    {
        var userId = User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(messageParams.FamilyId, userId))
            return BadRequest("You are not a member of this family");

        var messages = await _uow.MessageRepository.GetMessagesForFamily(messageParams);

        Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize,
            messages.TotalCount, messages.TotalPages));

        return messages;
    }

    [HttpGet("thread/{familyId}")]
    public async Task<ActionResult<PagedList<MessageDto>>> GetFamilyMessageThread(int familyId)
    {
        var userId = User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(familyId, userId))
            return BadRequest("You are not a member of this family");

        return Ok(await _uow.MessageRepository.GetFamilyMessageThread(familyId));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var userId = User.GetUserId();

        var message = await _uow.MessageRepository.GetMessage(id);

        if (message.SenderId != userId) return Unauthorized();

        if (!await _uow.FamilyRepository.IsFamilyMember(message.FamilyId, userId))
            return BadRequest("You are not a member of this family");

        message.SenderDeleted = true;

        //not going to delete messages entirely, only not showing (delete maby when admin or family owner deletes it to ??)
        //maby make functionaliy of retriving messages ??
        //maby show that a message has been deleted ??
        // if (message.SenderDeleted)
        // {
        //     _uow.MessageRepository.DeleteMessage(message);
        // }

        if (await _uow.Complete()) return Ok();

        return BadRequest("Problem deleting message");
    }
}
