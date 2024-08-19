namespace API.DTOs;

public class FamilyListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ListItemDto> ListItems { get; set; }
}