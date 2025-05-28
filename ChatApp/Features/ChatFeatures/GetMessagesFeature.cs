using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ChatApp.Shared.Exceptions;
using ChatApp.Shared.Utils;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ChatApp.Features.ChatFeatures.GetMessageFeature;
using ChatApp.Features.ChatFeatures.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using ChatApp.Shared.Services.ChatServices;
using ChatApp.Database.Entities;
using ChatApp.Shared.Services.Auth;
using FirebaseAdmin.Auth;

namespace ChatApp.Features.ChatFeatures;

public static class GetMessageFeature
{
    public class Handler : IScoped
    {
        private readonly IChatService _chatService;
        private readonly IAuthService _authService;
        public Handler(IChatService chatService, IAuthService authService)
        {
            _chatService = chatService;
            _authService = authService;
        }
        public async Task<List<Message>> Handle(Guid chatId, CancellationToken cancellationToken)
        {
            Guid userId = _authService.GetUserId();
            List<Message> messages = await _chatService.GetMessages(chatId, userId);
            return messages;
        }
    }
}

public class GetMessageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/ChatFeatures/Messages", async (Guid chatId, Handler handler, CancellationToken cancellationToken) =>
        {
            List<Message> messages = await handler.Handle(chatId, cancellationToken);
            return Results.Ok(messages);
        })
        .WithTags("ChatFeatures")
        .WithMetadata(new AuthorizeAttribute { Roles = "User" })
        .RequireAuthorization();
    }
}
