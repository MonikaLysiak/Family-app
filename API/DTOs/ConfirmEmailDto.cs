namespace API.DTOs;

public class ConfirmEmailDto
{
    public required string UserId { get; set; }
    public required string Token { get; set; }
}
