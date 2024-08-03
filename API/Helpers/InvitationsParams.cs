namespace API.Helpers;

public class InvitationsParams : PaginationParams
{
    public int UserId { get; set; }
    public string Predicate { get; set; }
}
