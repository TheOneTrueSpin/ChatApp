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

public class ChatService : IChatService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    public ChatService(IMessageRepository messageRepository, IUnitOfWork unitOfWork, IAuthService authService, IChatRepository chatRepository, IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _messageRepository = messageRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _userRepository = userRepository;

    }
    public async Task SendMessage(MessageRequestDto messageRequestDto)
    {
        Guid userId = _authService.GetUserId();
        Chat? chat = await GetChat(messageRequestDto.ChatId, userId);
        if (chat is null)
        {
            throw new ApiException("The Chat does not exist or the user is not a part of this chat", HttpStatusCode.BadRequest);
        }

        Message message = new Message()
        {
            Id = Guid.NewGuid(),
            SentOnUTC = DateTime.UtcNow,
            MessageContents = messageRequestDto.Message,
            SenderId = userId,
            ChatId = messageRequestDto.ChatId
        };
        _messageRepository.Add(message);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<Chat?> GetChat(Guid chatId, Guid userId)
    {
        Chat? chat = await _chatRepository.GetChat(chatId, userId);
        return chat;
    }
    public async Task CreateChat(List<Guid> userIds)
    {
        Guid userId = _authService.GetUserId();
        if (!userIds.Contains(userId))
        {
            throw new ApiException("User does not have permission to create this group (They arnt in it)", HttpStatusCode.BadRequest);
        }
        List<User> participants = await _userRepository.GetUsersByUserIds(userIds);
        if (participants.Count != userIds.Count)
        {
            throw new ApiException("One or more users were not returned", HttpStatusCode.BadRequest);
        }

        Chat newChat = new Chat()
        {
            Id = Guid.NewGuid(),
            Participants = participants

        };

        _chatRepository.Add(newChat);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<List<Message>> GetMessages(Guid chatId, Guid userId)
    {
        Guid currentUserId = _authService.GetUserId();
        if (currentUserId != userId)
        {
            throw new ApiException("User is requesting messages they dont own", HttpStatusCode.BadRequest);
        }
        Chat? chat = await GetChat(chatId, userId);
        if (chat is null)
        {
            throw new ApiException("The chat is null", HttpStatusCode.BadRequest);
        }
        List<Message> messages = chat.Messages;
        return messages;
    }
    

}