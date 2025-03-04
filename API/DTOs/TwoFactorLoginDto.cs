namespace API.DTOs;

public class TwoFactorLoginDto
{
    public required string UserName { get; set; }
    public required string TwoFactorCode { get; set; }
}
