using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database;
using ChatApp.Database.Entities;
using ChatApp.Shared.Repositories.Interfaces;

namespace ChatApp.Shared.Repositories;

public class ChatRepository:IChatRepository
{
    private readonly DataContext _dataContext;
    public ChatRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
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

