using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces;

public interface IAccountService
{
    Task<IdentityResult> RegisterUserAsync(AppUser user, string password, IEnumerable<string> roles);
    Task<IdentityResult> AddUserToRolesAsync(AppUser user, IEnumerable<string> roles);
    Task<IdentityResult> RemoveUserFromRolesAsync(AppUser user, IEnumerable<string> roles);
    Task<AuthResponse> SendConfirmationEmailAsync(AppUser user);
    Task<AuthResponse?> LoginAsync(AppUser user, string password);
    Task<AuthResponse> TwoFactorLoginAsync(AppUser user, string twoFactorCode);
    Task<bool> UserExistsAsync(string username);
}