using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ListsRepository : IListsRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ListsRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    // same in messeges repo, maby add hepler or service later on ?? or ConnectionRepo ??
    // also must add messeges / lists to group name so that there wont be same name for both (as for now it is only an Id)
    public void AddGroup(Group group)
    {
        _context.Groups.Add(group);
    }

    public void AddList(FamilyList familyList)
    {
        _context.Lists.Add(familyList);
    }

    public void DeleteList(FamilyList familyList)
    {
        _context.Lists.Remove(familyList);
    }

    // same in messeges repo, maby add hepler or service later on ?? or ConnectionRepo ??
    public async Task<Connection> GetConnection(string connectionId)
    {
        return await _context.Connections.FindAsync(connectionId);
    }

    // same in messeges repo, maby add hepler or service later on ?? or ConnectionRepo ??
    public async Task<Group> GetGroupForConnection(string connectionId)
    {
        return await _context.Groups
            .Include(x => x.Connections)
            .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
            .FirstOrDefaultAsync();
    }

    public async Task<FamilyList> GetList(int id)
    {
        return await _context.Lists.FindAsync(id);
    }

    public async Task<FamilyList> GetListWithItems(int id)
    {
        return await _context.Lists.Where(x => x.Id == id).Include(x => x.ListItems).FirstOrDefaultAsync();
    }

    public async Task<Group> GetListGroup(string groupName)
    {
        return await _context.Groups
            .Include(x => x.Connections)
            .FirstOrDefaultAsync(x => x.Name == groupName);
    }


    public async Task<IEnumerable<FamilyListDto>> GetFamilyLists(int familyId)
    {
        var query = _context.Lists
            .Where(
                m => m.FamilyId == familyId
            )
            .OrderByDescending(m => m.Created)
            .Include(x => x.ListItems.OrderByDescending(m => m.Created).Take(5))
            .AsQueryable();

        return await query.ProjectTo<FamilyListDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int categoryId) {
        return await _context.Categories.FindAsync(categoryId);
    }
    
    // same in messeges repo, maby add hepler or service later on ?? or ConnectionRepo ??
    public void RemoveConnection(Connection connection)
    {
        _context.Connections.Remove(connection);
    }
}