using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class FamilyController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public FamilyController(IMapper mapper, IUnitOfWork uow)
    {
        _uow = uow;
        _mapper = mapper;
    }

    [HttpPost("{familyName}")]
    public async Task<ActionResult<FamilyDto>> CreateFamily(string familyName)
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
    
    [HttpGet]
    public async Task<ActionResult<PagedList<FamilyDto>>> GetFamilies([FromQuery]FamilyParams familyParams)
    {
        familyParams.CurrentUserId = User.GetUserId();

        var families = await _uow.FamilyRepository.GetUserFamiliesAsync(familyParams);

        Response.AddPaginationHeader(new PaginationHeader(families.CurrentPage, families.PageSize, families.TotalCount, families.TotalPages));

        return Ok(families);
    }
    
    [HttpGet("members")]
    public async Task<ActionResult<MemberDto>> GetFamilyMembers([FromQuery]FamilyMemberParams familyMemberParams)
    {
        var currentUserId = User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(familyMemberParams.FamilyId, currentUserId))
            return BadRequest("You are not a member of this family");
            
        var familyMembers = await _uow.FamilyMemberRepository.GetFamilyMembersAsync(familyMemberParams);

        return Ok(familyMembers);
    }
    
    [HttpGet("{familyId}/{familyMemberId}")]
    public async Task<ActionResult<MemberDto>> GetFamilyMember(int familyId, int familyMemberId)
    {
        var currentUserId = User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(familyId, currentUserId))
            return BadRequest("You are not a member of this family");

        if (!await _uow.FamilyRepository.IsFamilyMember(familyId, familyMemberId))
            return BadRequest("This user is not a member of this family");
            
        var familyMember = await _uow.UserRepository.GetMemberAsync(familyMemberId);

        familyMember.Nickname = await _uow.FamilyMemberRepository.GetFamilyMemberNickname(familyId, familyMemberId);

        return Ok(familyMember);
    }
}