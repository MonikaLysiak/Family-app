namespace API.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IMessageRepository MessageRepository { get; }
    IInvitationsRepository LikesRepository { get; }
    IFamilyRepository FamilyRepository { get; }
    
    Task<bool> Complete();
    bool HasChanges();
}
