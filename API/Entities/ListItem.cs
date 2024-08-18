using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Items")]
public class ListItem
{
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public string Content { get; set; }
    public bool IsChecked { get; set; } = false;
    
    public int FamilyListId { get; set; }
    public FamilyList familyList { get; set; }
}
