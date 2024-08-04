using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class FamilyController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public FamilyController(IMapper mapper, IUnitOfWork uow)
    {
        _uow = uow;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<FamilyDto>> CreateFamily([FromQuery]string familyName)
    {
        var username = User.GetUsername();

        var user = await _uow.UserRepository.GetUserByUsernameAsync(username);

        // add Create property for family
        var family = new Family
        {
            Name = familyName
        };

        _uow.FamilyRepository.AddFamily(family);

        if (!await _uow.Complete()) return BadRequest("Failed to create family");
        
        var userFamily = new AppUserFamily
        {
            UserId = user.Id,
            FamilyId = family.Id
        };

        user.UserFamilies.Add(userFamily);
        // why not two ads in invitations / likes ??
        // should i remove the second add ??
        family.UserFamilies.Add(userFamily);
        
        if (await _uow.Complete()) return Ok(_mapper.Map<FamilyDto>(family));

        return BadRequest("Failed add user to family");
    }

    
}