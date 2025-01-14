﻿using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("register")] // POST: api//account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExists(registerDto.UserName)) return BadRequest("Username is taken");

        var user = _mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.UserName.ToLower();

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, "Member");

        if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

        return new UserDto 
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
            Name = user.Name
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .Include(p => p.UserPhotos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

        if(user == null) return Unauthorized("invalid username");

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result) return Unauthorized("Invalid password");

        return new UserDto 
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
            PhotoUrl = user.UserPhotos.FirstOrDefault(x => x.IsMain)?.Url,
            Name = user.Name
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
