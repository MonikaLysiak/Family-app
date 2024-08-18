namespace API.DTOs;

public class CreateListDto
{
    public int FamilyId { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public List<string> ListItems { get; set; } = [];
}