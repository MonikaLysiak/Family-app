namespace API.Entities;

public class Invitation
{
    public int Id { get; set;}
    
    public AppUser InviterUser { get; set; }
    public int InviterUserId { get; set; }

    public AppUser InviteeUser { get; set; }
    public int InviteeUserId { get; set; }

    public Family Family { get; set; }
    public int FamilyId { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;
}
