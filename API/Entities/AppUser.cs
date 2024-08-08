using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser : IdentityUser<int>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; }
    public DateTime LastActive { get; set;} = DateTime.UtcNow;

    public List<UserPhoto> UserPhotos { get; set; } = [];

    public List<AppUserFamily> UserFamilies { get; set; } = [];

    public List<Invitation> InvitationsReceived { get; set; } = [];
    public List<Invitation> InvitationsSent { get; set; } = [];

    public ICollection<Message> MessagesSent { get; set; }

    public ICollection<AppUserRole> UserRoles { get; set; }

    public ICollection<FamilyPhoto> FamilyPhots { get; set; }
}