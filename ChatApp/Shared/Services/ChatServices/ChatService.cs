using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database.Entities;
using ChatApp.Features.ChatFeatures.Dtos;


namespace ChatApp.Shared.Services.ChatServices;
public class ChatService:IChatService
{
    public async Task SendMessage(MessageRequestDto messageRequestDto)
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