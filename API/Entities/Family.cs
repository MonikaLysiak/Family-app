namespace API.Entities;

public class Family
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<FamilyPhoto> FamilyPhotos { get; set; } = new();

    public ICollection<AppUserFamily> UserFamilies { get; set; }

    public ICollection<Invitation> FamilyInvitations { get; set; }

    public ICollection<Message> Messages { get; set; }
}