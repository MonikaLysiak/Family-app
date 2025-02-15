using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IListsRepository
{
    void AddGroup(Group group);
    void AddList(FamilyList familyList);
    void DeleteList(FamilyList familyList);
    Task<Connection> GetConnectionAsync(string connectionId);
    Task<Group> GetGroupForConnectionAsync(string connectionId);
    Task<FamilyList> GetListAsync(int id);
    Task<FamilyList> GetListWithItemsAsync(int id);
    Task<Group> GetListGroupAsync(string groupName);
    Task<IEnumerable<FamilyListDto>> GetFamilyListsAsync(int familyId);
    Task<Category> GetCategoryByIdAsync(int categoryId);
    void RemoveConnection(Connection connection);
}