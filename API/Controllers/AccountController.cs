using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper, IEmailService emailService) : BaseApiController
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IMapper _mapper = mapper;
    private readonly IEmailService _emailService = emailService;

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExistsAsync(registerDto.UserName)) return BadRequest("Username is taken");

        var user = _mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.UserName.ToLower();

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var confirmationLink = Url.PageLink(pageName: "/Account/ConfirmEmail",
            values: new { userId = user.Id, token = confirmationToken });

        await _emailService.SendFromFamilyAppAsync(
            user.Email, 
            "Confirm your email", 
            $"Please confirm your email by clicking this link: {confirmationLink}"
            );

        var roleResult = await _userManager.AddToRoleAsync(user, "Member");

        if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

        return new UserDto 
        {
            Username = user.UserName,
            Token = await _tokenService.CreateTokenAsync(user),
            Name = user.Name
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .Include(p => p.UserPhotos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

        if(user == null) return Unauthorized("Invalid username");

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result) return Unauthorized("Invalid password");

        return new UserDto 
        {
            Username = user.UserName,
            Token = await _tokenService.CreateTokenAsync(user),
            PhotoUrl = user.UserPhotos.FirstOrDefault(x => x.IsMain)?.Url,
            Name = user.Name
        };
    }

    private async Task<bool> UserExistsAsync(string username)
    {
        return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
