using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

[Authorize]
public class ListsHub : Hub
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ListsHub(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        
        if (!int.TryParse(httpContext.Request.Query["familyId"], out var familyId))
            throw new HubException("'familyId' must exist as an integer");

        if (!int.TryParse(httpContext.Request.Query["listId"], out var listId))
            throw new HubException("'listId' must exist as an integer");

        var userId = Context.User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(familyId, userId))
            throw new HubException("You are not a member of this family");

        var groupName = GetGroupName(familyId, listId);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var group = await AddToGroup(groupName);

        await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

        var messages = await _uow.MessageRepository.GetFamilyMessageThread(familyId);

        if (_uow.HasChanges()) await _uow.Complete();

        await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var group = await RemoveFromFamilyListGroup();
        await Clients.Group(group.Name).SendAsync("UpdatedGroup");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task UpdateFamilyList(ListItemDto listItemDto)
    {
        var userId = Context.User.GetUserId();

        var familyList = await _uow.ListsRepository.GetListWithItems(listItemDto.FamilyListId);

        if (familyList == null)
            throw new HubException("There is no family list of that id");

        var listItem = familyList.ListItems.FirstOrDefault(x => x.Id == listItemDto.Id) ?? new ListItem();

        if (!await _uow.FamilyRepository.IsFamilyMember(familyList.FamilyId, userId))
            throw new HubException("You are not a member of this family");

        var modifier = await _uow.UserRepository.GetUserByIdAsync(userId);
        var family = await _uow.FamilyRepository.GetFamilyByIdAsync(familyList.FamilyId) ?? throw new HubException("Not found family");
        
        listItem.Content = listItemDto.Content;
        listItem.IsChecked = listItemDto.IsChecked;

        var groupName = GetGroupName(familyList.FamilyId, listItemDto.FamilyListId);

        var group = await _uow.ListsRepository.GetListGroup(groupName);

        if (await _uow.Complete())
        {
            await Clients.Group(groupName).SendAsync("ListItemUpdated", _mapper.Map<ListItemDto>(listItem));
        }
    }

    private async Task<Group> AddToGroup(string groupName)
    {
        var group = await _uow.ListsRepository.GetListGroup(groupName);
        var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

        if (group == null)
        {
            group = new Group(groupName);
            _uow.ListsRepository.AddGroup(group);
        }

        group.Connections.Add(connection);

        if (await _uow.Complete()) return group;

        throw new HubException("Failed to add to group");
    }

    private async Task<Group> RemoveFromFamilyListGroup()
    {
        var group = await _uow.ListsRepository.GetGroupForConnection(Context.ConnectionId);
        var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        _uow.ListsRepository.RemoveConnection(connection);
        if (await _uow.Complete()) return group;

        throw new HubException("Failed to remove from group");
    }

    private string GetGroupName(int familyId, int listId) {
        return "list" + familyId + "_" + listId;
    }
}
