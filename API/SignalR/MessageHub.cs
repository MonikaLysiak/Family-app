using System.Numerics;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

[Authorize]
public class MessageHub : Hub
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IHubContext<PresenceHub> _presenceHub;

    public MessageHub(IUnitOfWork uow, IMapper mapper, IHubContext<PresenceHub> presenceHub)
    {
        _uow = uow;
        _mapper = mapper;
        _presenceHub = presenceHub;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        
        if (!int.TryParse(httpContext.Request.Query["familyId"], out var familyId))
            throw new HubException("'familyId' must exist as an integer");

        var userId = Context.User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(familyId, userId))
            throw new HubException("You are not a member of this family");

        var groupName = GetGroupName(familyId);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var group = await AddToGroup(groupName);

        await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

        var messages = await _uow.MessageRepository.GetFamilyMessageThread(familyId);

        if (_uow.HasChanges()) await _uow.Complete();

        await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var group = await RemoveFromMessageGroup();
        await Clients.Group(group.Name).SendAsync("UpdatedGroup");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(CreateMessageDto createMessageDto)
    {
        var userId = Context.User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(createMessageDto.FamilyId, userId))
            throw new HubException("You are not a member of this family");

        var sender = await _uow.UserRepository.GetUserWithPhotosByIdAsync(userId);
        var family = await _uow.FamilyRepository.GetFamilyByIdAsync(createMessageDto.FamilyId) ?? throw new HubException("Not found user");
        
        var message = new Message
        {
            SenderId = sender.Id,
            SenderUsername = sender.UserName,
            Sender = sender,

            FamilyId = family.Id,
            Family = family,
            
            Content = createMessageDto.Content
        };

        var groupName = GetGroupName(createMessageDto.FamilyId);

        _uow.MessageRepository.AddMessage(message);

        if (await _uow.Complete())
        {
            await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
        }
    }

    private async Task<Group> AddToGroup(string groupName)
    {
        var group = await _uow.MessageRepository.GetMessageGroup(groupName);
        var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

        if (group == null)
        {
            group = new Group(groupName);
            _uow.MessageRepository.AddGroup(group);
        }

        group.Connections.Add(connection);

        if (await _uow.Complete()) return group;

        throw new HubException("Failed to add to group");
    }

    private async Task<Group> RemoveFromMessageGroup()
    {
        var group = await _uow.MessageRepository.GetGroupForConnection(Context.ConnectionId);
        var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        _uow.MessageRepository.RemoveConnection(connection);
        if (await _uow.Complete()) return group;

        throw new HubException("Failed to remove from group");
    }

    private string GetGroupName(int familyId) {
        return "chat" + familyId;
    }
}
