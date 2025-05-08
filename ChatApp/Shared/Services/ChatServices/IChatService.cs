using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database.Entities;
using ChatApp.Features.ChatFeatures.Dtos;


namespace ChatApp.Shared.Services.ChatServices;

public interface IChatService
{
    public Task SendMessage(MessageRequestDto messageRequestDto);
    public Task<Chat?> GetChat(Guid chatId, Guid userId);
    public  Task CreateChat(List<User> users);
    public Task<List<Message>> GetMessages(Guid chatId, Guid userId);

}
