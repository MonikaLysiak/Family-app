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
    private readonly IPhotoService _photoService;

    public FamilyController(IUnitOfWork uow, IMapper mapper, IPhotoService photoService)
    {
        _uow = uow;
        _mapper = mapper;
        _photoService = photoService;
    }

    [HttpPost("add-family/{familyName}")]
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

    [HttpGet("{familyId}")]
    public async Task<ActionResult<MemberDto>> GetFamilyDetails(int familyId)
    {
        var currentUserId = User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(familyId, currentUserId))
            return BadRequest("You are not a member of this family");

        var family = await _uow.FamilyRepository.GetFamilyDetailsAsync(familyId);

        return Ok(family);
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
    
    [HttpPost("add-photo/{familyId}")]
    public async Task<ActionResult<PhotoDto>> AddFamilyPhoto(int familyId, IFormFile file)
    {
        var userId = User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(familyId, userId))
            return BadRequest("You are not a member of this family");
            
        var family = await _uow.FamilyRepository.GetFamilyWithPhotosByIdAsync(familyId);

        if (family == null) return NotFound();

        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new FamilyPhoto
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            AuthorId = userId
        };

        if (family.FamilyPhotos.Count == 0) photo.IsMain = true;

        family.FamilyPhotos.Add(photo);

        if (await _uow.Complete()) 
        {
            return CreatedAtAction(nameof(GetFamilyDetails), new {familyId = family.Id}, _mapper.Map<PhotoDto>(photo));
        }

        return BadRequest("Problem adding family photo");
    }
    
    [HttpPut("set-main-photo/{familyId}/{photoId}")]
    public async Task<ActionResult> SetMainFamilyPhoto(int familyId, int photoId)
    {
        var userId = User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(familyId, userId))
            return BadRequest("You are not a member of this family");

        var family = await _uow.FamilyRepository.GetFamilyWithPhotosByIdAsync(familyId);

        if (family == null) return NotFound();

        var photo = family.FamilyPhotos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("this is already your main photo");

        var currentMain = family.FamilyPhotos.FirstOrDefault(x => x.IsMain);
        if (currentMain !=null) currentMain.IsMain = false;
        photo.IsMain = true;

        if (await _uow.Complete()) return NoContent();

        return BadRequest("Problem setting the main photo");
    }

    [HttpDelete("delete-photo/{familyId}/{photoId}")]
    public async Task<ActionResult> DeleteFamilyPhoto(int familyId, int photoId)
    {
        var userId = User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(familyId, userId))
            return BadRequest("You are not a member of this family");

        var family = await _uow.FamilyRepository.GetFamilyWithPhotosByIdAsync(familyId);

        if (family == null) return NotFound();

        var photo = family.FamilyPhotos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("You cannot delete family's main photo");

        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        family.FamilyPhotos.Remove(photo);

        if (await _uow.Complete()) return Ok();

        return BadRequest("Problem deleting photo");
    }
}