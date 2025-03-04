using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public required string UserName { get; set; }

    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Surname { get; set; }

    [Required]
    public DateOnly? DateOfBirth { get; set; }

    [Required]
    [StringLength(8, MinimumLength = 4)]
    public required string Password { get; set; }
}