namespace API.DTOs;

public class FamilyDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhotoUrl { get; set;}
    public List<PhotoDto> FamilyPhotos { get; set; }
    public string UserNickname { get; set; }
}