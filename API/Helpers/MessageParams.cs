namespace API.Helpers;

public class MessageParams : PaginationParams
{
    public int FamilyId { get; set; }
    public string Container { get; set; } = "Unread";
}
