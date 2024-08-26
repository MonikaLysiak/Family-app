namespace API.Entities;

public class Family
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;

    public List<FamilyPhoto> FamilyPhotos { get; set; } = [];

    public List<AppUserFamily> UserFamilies { get; set; } = [];

    public ICollection<Invitation> FamilyInvitations { get; set; }

    public List<Message> Messages { get; set; } = [];
}