namespace API.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IMessageRepository MessageRepository { get; }
    IInvitationsRepository InvitationsRepository { get; }
    IFamilyRepository FamilyRepository { get; }
    IFamilyMemberRepository FamilyMemberRepository { get; }
    
    Task<bool> Complete();
    bool HasChanges();
}
