using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database;
using ChatApp.Database.Entities;
using ChatApp.Shared.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Shared.Repositories;

public class ChatRepository:IChatRepository
{
    private readonly DataContext _dataContext;
    public ChatRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<Chat?> GetChat(Guid chatId, Guid userId)
    {
        Chat? chat = await _dataContext.Chats.FirstOrDefaultAsync(c => chatId == c.Id && c.Participants.Any(p => p.Id == userId));
        return chat;
    }
    public void Add(Chat chat)
    {
        _dataContext.Set<Chat>().Add(chat);
    }

    public void Remove(Chat chat)
    {
        _dataContext.Set<Chat>().Remove(chat);
    }
    
}

