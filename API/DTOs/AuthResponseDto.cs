using API.Enums;

namespace API.DTOs;
public class AuthResponse
{
    public AuthStatus Status { get; set; }
    public UserDto? User { get; set; }
}
