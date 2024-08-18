namespace API.Helpers;

public class FamilyListsParams : PaginationParams
{
    public int FamilyId { get; set; }
    
    public string OrderBy { get; set; } = "lastActive";
}
