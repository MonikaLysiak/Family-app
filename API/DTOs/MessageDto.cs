namespace API.DTOs;

public class MessageDto
{
    public int Id { get; set; }
    
    public int SenderId { get; set; }
    public string SenderUsername { get; set; }
    public string SenderPhotoUrl { get; set; }

    public int FamilyId { get; set; }
    public string FamilyName { get; set; }
    public string FamilyPhotoUrl { get; set; }

    public string Content { get; set; }
    public DateTime MessageSent { get; set; }
    public bool SenderDeleted { get; set; }
}
