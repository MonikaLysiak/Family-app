namespace API.Helpers;

public class FamilyMemberParams : PaginationParams
{
    public int FamilyId { get; set; }
    public string OrderBy { get; set; } = "created";
}
