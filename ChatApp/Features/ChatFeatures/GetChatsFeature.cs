using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carter;
using ChatApp.Features.ChatFeatures.Dtos;
using ChatApp.Shared.Services.ChatServices;
using ChatApp.Shared.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using static ChatApp.Features.ChatFeatures.GetChatsFeature;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Database.Entities;
using ChatApp.Shared.Services.Auth;

namespace ChatApp.Features.ChatFeatures;

public static class GetChatsFeature
{
    public class GetChatsResponse
    {
        public required List<ChatResponseDto> Chats { get; set; }
    }
    public class Handler : IScoped
    {
        private readonly IChatService _chatService;
        private readonly IAuthService _authService;
        public Handler(IChatService chatService, IAuthService authService)
        {
            _chatService = chatService;
            _authService = authService;
        }
        public async Task<GetChatsResponse> Handle(CancellationToken cancellationToken)
        {
            Guid userId = _authService.GetUserId();
            List<Chat> chats = await _chatService.GetChats(userId);
            
            List<ChatResponseDto> chatResponseDtoList = chats.Select(chat =>
            {
                ChatResponseDto chatResponse = new ChatResponseDto()
                {
                    Id = chat.Id
                };
                return chatResponse;
            }).ToList();

            return new GetChatsResponse()
            {
                Chats = chatResponseDtoList
            };
        }
    }
}
