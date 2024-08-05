namespace API.Helpers;

public class FamilyParams : PaginationParams
{
    public int CurrentUserId { get; set; }
    
    public string OrderBy { get; set; } = "lastActive";
}
