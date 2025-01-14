﻿using API.Interfaces;
using AutoMapper;

namespace API.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UnitOfWork(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public IUserRepository UserRepository => new UserRepository(_context, _mapper);

    public IMessageRepository MessageRepository => new MessageRepository(_context, _mapper);

    public IInvitationsRepository InvitationsRepository => new InvitationsRepository(_context);

    public IFamilyRepository FamilyRepository => new FamilyRepository(_context, _mapper);

    public IFamilyMemberRepository FamilyMemberRepository => new FamilyMemberRepository(_context, _mapper);
    
    public IListsRepository ListsRepository => new ListsRepository(_context, _mapper);

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }
}
