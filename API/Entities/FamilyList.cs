namespace API.Entities;

public class FamilyList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    
    public int FamilyId { get; set; }
    public Family Family { get; set; }
    
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public int AuthorId { get; set; }
    public AppUser Author { get; set; }

    public List<ListItem> ListItems { get; set; } = [];
}
