using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikesRepository : ILikesRepository
{
    private readonly DataContext _context;
    public LikesRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<Invitation> GetUserLike(int sourceUserId, int targetUserId)
    {
        return await _context.Invitations.FindAsync(sourceUserId, targetUserId);
    }

    //to be fixed
    //when likesParams.Predicate != "liked" and "likedBy" it returnes all users
    public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
    {
        var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
        var likes = _context.Invitations.AsQueryable();

        if (likesParams.Predicate == "liked")
        {
            likes = likes.Where(like => like.InviterUserId == likesParams.UserId);
            users = likes.Select(like => like.InviteeUser);
        }

        if (likesParams.Predicate == "likedBy")
        {
            likes = likes.Where(like => like.InviteeUserId == likesParams.UserId);
            users = likes.Select(like => like.InviterUser);
        }

        var likedUsers = users.Select(user => new LikeDto
        {
            UserName = user.UserName,
            KnownAs = user.Name,
            Age = user.DateOfBirth.CalculateAge(),
            PhotoUrl = user.UserPhotos.FirstOrDefault(x => x.IsMain).Url,
            City = user.City,
            Id = user.Id
        });

        return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
    }

    public async Task<AppUser> GetUserWithLikes(int userId)
    {
        return await _context.Users
            .Include(x => x.InvitationsSent)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }
}
