using API.DTOs;
using API.Entities;
using API.Enums;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IAccountService _accountService;

    public AccountController(UserManager<AppUser> userManager, IMapper mapper, IAccountService accountService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        if (await _accountService.UserExistsAsync(registerDto.UserName)) return BadRequest("Username is taken");

        var user = _mapper.Map<AppUser>(registerDto);

        var result = await _accountService.RegisterUserAsync(user, registerDto.Password, ["Member"]);

        if (!result.Succeeded) return BadRequest(result.Errors);

        if (user.Email == null) return BadRequest("User has no email");

        return Ok(await _accountService.SendConfirmationEmailAsync(user));
    }

    [HttpPost("sendConfirmationEmail")]
    public async Task<ActionResult> SendConfirmationEmail(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null) return BadRequest("User not found");

        if (user.Email == null) return BadRequest("User has no email");

        return Ok(await _accountService.SendConfirmationEmailAsync(user));
    }


    [HttpPost("confirmEmail")]
    public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmailDto)
    {
        var user = await _userManager.FindByIdAsync(confirmEmailDto.UserId);

        if (user == null) return BadRequest("User not found");

        if (user.EmailConfirmed) return Ok(new AuthResponse { Status = AuthStatus.EmailAlreadyConfirmed });

        var result = await _userManager.ConfirmEmailAsync(user, confirmEmailDto.Token);

        if (!result.Succeeded) return Ok(new AuthResponse { Status = AuthStatus.InvalidConfirmationToken });

        return Ok(new AuthResponse { Status = AuthStatus.EmailConfirmed });
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .Include(p => p.UserPhotos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

        if (user == null) return Ok(new AuthResponse { Status = AuthStatus.InvalidCredentials });
        
        if (user.Email == null) return BadRequest("User has no email");

        var result = await _accountService.LoginAsync(user, loginDto.Password);

        if (result == null) return Unauthorized("Failed to login");

        return Ok(result);
    }

    [HttpPost("twoFactorLogin")]
    public async Task<ActionResult<UserDto>> TwoFactorLogin([FromBody] TwoFactorLoginDto twoFactorLoginDto)
    {
        var user = await _userManager.Users
            .Include(p => p.UserPhotos)
            .SingleOrDefaultAsync(x => x.UserName == twoFactorLoginDto.UserName);

        if (user == null) return Ok(new AuthResponse { Status = AuthStatus.InvalidCredentials });

        return Ok(await _accountService.TwoFactorLoginAsync(user, twoFactorLoginDto.TwoFactorCode));
    }
}
