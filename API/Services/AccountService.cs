using System.Web;
using API.DTOs;
using API.Entities;
using API.Enums;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;

    public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _emailService = emailService;
    }

    public async Task<IdentityResult> RegisterUserAsync(AppUser user, string password, IEnumerable<string> roles)
    {
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded) return result;

        return await AddUserToRolesAsync(user, ["Member"]);
    }

    public async Task<IdentityResult> AddUserToRolesAsync(AppUser user, IEnumerable<string> roles)
    {
        return await _userManager.AddToRolesAsync(user, roles);
    }

    public async Task<IdentityResult> RemoveUserFromRolesAsync(AppUser user, IEnumerable<string> roles)
    {
        return await _userManager.RemoveFromRolesAsync(user, roles);
    }

    public async Task<AuthResponse> SendConfirmationEmailAsync(AppUser user)
    {
        if (user.EmailConfirmed) return new AuthResponse { Status = AuthStatus.EmailAlreadyConfirmed };

        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        confirmationToken = HttpUtility.UrlEncode(confirmationToken);

        var confirmationUrl = $"https://localhost:4200/confirmEmail?userId={user.Id}&token={confirmationToken}";

        await _emailService.SendFromFamilyAppAsync(
            user.Email,
            "Confirm your email",
            $"Please confirm your email by clicking this link: {confirmationUrl}"
            );

        return new AuthResponse { Status = AuthStatus.EmailConfirmationSent };
    }

    public async Task<AuthResponse?> LoginAsync(AppUser user, string password) 
    {
        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

        if (result.Succeeded)
            return new AuthResponse
            {
                Status = AuthStatus.LoggedIn,
                User = new UserDto
                {
                    Username = user.UserName,
                    Token = await _tokenService.CreateTokenAsync(user),
                    PhotoUrl = user.UserPhotos.FirstOrDefault(x => x.IsMain)?.Url,
                    Name = user.Name
                }
            };
        else
        {
            if (result.IsLockedOut)
                return new AuthResponse { Status = AuthStatus.LockedOut };
            if (result.RequiresTwoFactor)
            {
                var securityCode = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                await _emailService.SendFromFamilyAppAsync(
                    user.Email,
                    "Family App's OTP",
                    $"Please use this code as the OTP: {securityCode}");

                return new AuthResponse
                {
                    Status = AuthStatus.TwoFactorRequired,
                    User = new UserDto
                    {
                        Username = user.UserName,
                        Token = await _tokenService.CreateTokenAsync(user),
                        PhotoUrl = user.UserPhotos.FirstOrDefault(x => x.IsMain)?.Url,
                        Name = user.Name
                    }
                };
            }
            if (result.IsNotAllowed)
                return null;
            else
                return new AuthResponse { Status = AuthStatus.InvalidCredentials };
        }
    }

    public async Task<AuthResponse> TwoFactorLoginAsync(AppUser user, string twoFactorCode)
    {
        var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(twoFactorCode, false, false);

        if (result.Succeeded)
            return new AuthResponse
            {
                Status = AuthStatus.LoggedIn,
                User = new UserDto
                {
                    Username = user.UserName,
                    Token = await _tokenService.CreateTokenAsync(user),
                    PhotoUrl = user.UserPhotos.FirstOrDefault(x => x.IsMain)?.Url,
                    Name = user.Name
                }
            };

        return new AuthResponse { Status = AuthStatus.InvalidTwoFactorCode };
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}