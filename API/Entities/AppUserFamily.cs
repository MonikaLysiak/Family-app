namespace API.Entities;

public class AppUserFamily
{
    public int UserId { get; set; }
    public AppUser User { get; set; }

    public int FamilyId { get; set; }
    public Family Family { get; set; }

    public string Nickname { get; set; }
}
