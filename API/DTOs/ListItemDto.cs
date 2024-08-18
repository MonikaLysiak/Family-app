namespace API.DTOs;

public class ListItemDto
{
    public int? Id { get; set;}
    public int FamilyListId { get; set;}
    public string Content { get; set; }
    public bool IsChecked { get; set; } = false;
}
