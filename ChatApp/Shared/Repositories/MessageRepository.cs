using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database;
using ChatApp.Database.Entities;
using ChatApp.Shared.Repositories.Interfaces;

namespace ChatApp.Shared.Repositories;

public class MessageRepository:IMessageRepository
{
    private readonly DataContext _dataContext;
    public MessageRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public void Add(Message message)
    {
        _dataContext.Set<Message>().Add(message);
    }

    public void Remove(Message message)
    {
        _dataContext.Set<Message>().Remove(message);
    }
}

