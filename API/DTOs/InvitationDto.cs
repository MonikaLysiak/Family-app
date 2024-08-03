namespace API.DTOs;

public class InvitationDto
{
    public int Id { get; set; }

    public string UserName { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserPhotoUrl { get; set; }

    public string FamilyName { get; set; }
    public string FamilyPhotoUrl { get; set; }
}
