using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ChatApp.Database.Entities;
using ChatApp.Features.ChatFeatures.Dtos;
using ChatApp.Shared.Exceptions;
using ChatApp.Shared.Repositories.Interfaces;
using ChatApp.Shared.Services.Auth;
using ChatApp.Shared.Utils.UnitOfWork;
using Microsoft.IdentityModel.Tokens;


namespace ChatApp.Shared.Services.ChatServices;
public class ChatService:IChatService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    public ChatService(IMessageRepository messageRepository, IUnitOfWork unitOfWork, IAuthService authService)
    {
        _messageRepository = messageRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
        
    }
    public async Task SendMessage(MessageRequestDto messageRequestDto)
    {
        Guid userId = _authService.GetUserId();
        if (userId != messageRequestDto.SenderId)
        {
            throw new ApiException("The senderId does not match with the logged in user", HttpStatusCode.BadRequest);
        }

        
        Message message = new Message()
        {
            Id = Guid.NewGuid(),
            SentOnUTC = DateTime.UtcNow,
            MessageContents = messageRequestDto.Message,
            SenderId = messageRequestDto.SenderId,
            ChatId = messageRequestDto.ChatId
        };
        _messageRepository.Add(message);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<Chat> GetChat(Guid chatId, Guid userId)
    {
        //doin this for that next time
        throw new NotImplementedException();
    }
    public async Task NewChat(List<Guid> users)
    {
        throw new NotImplementedException();
    }

}