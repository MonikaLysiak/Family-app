namespace API.DTOs;

public class MemberDto
{
    public int Id { get; set; }
    public string UserName { get; set;}
    public string Nickname { get; set;}
    public string PhotoUrl { get; set;}
    public int Age { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastActive { get; set;}
    public string Gender { get; set; }
    public string Surname { get; set; }
    public List<PhotoDto> Photos { get; set; }
}
