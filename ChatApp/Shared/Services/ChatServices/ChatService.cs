using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database.Entities;


namespace ChatApp.Shared.Services.ChatServices;
public class ChatService
{
    public async Task SendMessage(Guid chatId, string message, Guid senderId)
    {
        throw new NotImplementedException();
    }
    public async Task<Chat> GetChat(Guid chatId, Guid userId)
    {
        throw new NotImplementedException();
    }
    public async Task NewChat(List<Guid> users)
    {
        throw new NotImplementedException();
    }

}