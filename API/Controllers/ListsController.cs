using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ListsController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public ListsController(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    [HttpPost]
    public async Task<ActionResult<FamilyListDto>> CreateFamilyList(CreateListDto createListDto)
    {
        var userId = User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(createListDto.FamilyId, userId))
            return BadRequest("You are not a member of this family");

        var author = await _uow.UserRepository.GetUserByIdAsync(userId);
        var family = await _uow.FamilyRepository.GetFamilyByIdAsync(createListDto.FamilyId);
        var category = await _uow.ListsRepository.GetCategoryByIdAsync(createListDto.CategoryId);

        if (author == null) return NotFound("There is no user of that id");
        if (family == null) return NotFound("There is no family of that id");

        var familyList = new FamilyList
        {
            Name = createListDto.Name,
            Author = author,
            Family = family,
            Category = category,
            AuthorId = author.Id,
            FamilyId = family.Id,
            CategoryId = createListDto.CategoryId
        };

        foreach (var listItem in createListDto.ListItems)
        {
            familyList.ListItems.Add(new ListItem
            {
                Content = listItem
            });
        }

        _uow.ListsRepository.AddList(familyList);

        if (await _uow.Complete()) return Ok(_mapper.Map<FamilyListDto>(familyList));

        return BadRequest("Failed to create family list");
    }

    [HttpPost("edit")]
    public async Task<ActionResult<FamilyListDto>> EditFamilyList(FamilyListDto editedList)
    {
        // ! must add family id and check if the user can edit this list
        var familyList = await _uow.ListsRepository.GetListWithItems(editedList.Id);

        if (familyList == null) return NotFound("There is no user of that id");

        familyList.ListItems.Clear();

        familyList.ListItems = [];

        var newItems = editedList.ListItems.Select(listItem => new ListItem
        {
            Content = listItem.Content,
            IsChecked = listItem.IsChecked,
            FamilyListId = familyList.Id
        }).ToList();

        familyList.ListItems.AddRange(newItems);

        if (await _uow.Complete()) return Ok(_mapper.Map<FamilyListDto>(familyList));

        return BadRequest("Failed to edit family list");
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<FamilyListDto>>> GetFamilyLists([FromQuery] FamilyListsParams familyListsParams)
    {
        var userId = User.GetUserId();

        if (!await _uow.FamilyRepository.IsFamilyMember(familyListsParams.FamilyId, userId))
            return BadRequest("You are not a member of this family");

        return Ok(await _uow.ListsRepository.GetFamilyLists(familyListsParams.FamilyId));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteList(int id)
    {
        var userId = User.GetUserId();

        var list = await _uow.ListsRepository.GetList(id);

        if (!await _uow.FamilyRepository.IsFamilyMember(list.FamilyId, userId))
            return BadRequest("You are not a member of this family");

        _uow.ListsRepository.DeleteList(list);

        if (await _uow.Complete()) return Ok();

        return BadRequest("Problem deleting family list");
    }
}