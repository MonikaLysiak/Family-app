namespace API.Entities;

public class Message
{
    public int Id { get; set; }

    public int SenderId { get; set; }
    public string SenderUsername { get; set; }
    public AppUser Sender { get; set; }
    
    public int FamilyId { get; set; }
    public Family Family { get; set; }
    
    public string Content { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    public bool SenderDeleted { get; set; }
}
