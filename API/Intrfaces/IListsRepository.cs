using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IListsRepository
{
    void AddGroup(Group group);
    void AddList(FamilyList familyList);
    void DeleteList(FamilyList familyList);
    Task<Connection> GetConnection(string connectionId);
    Task<Group> GetGroupForConnection(string connectionId);
    Task<FamilyList> GetList(int id);
    Task<FamilyList> GetListWithItems(int id);
    Task<Group> GetListGroup(string groupName);
    Task<IEnumerable<FamilyListDto>> GetFamilyLists(int familyId);
    Task<Category> GetCategoryByIdAsync(int categoryId);
    void RemoveConnection(Connection connection);
}