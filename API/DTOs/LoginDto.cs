namespace API.DTOs;

public class LoginDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
    //to add functionality to remember the user
    public bool RememberMe { get; set; }
}
