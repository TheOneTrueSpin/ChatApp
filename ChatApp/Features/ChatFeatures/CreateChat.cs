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
using static ChatApp.Features.ChatFeatures.CreateChatFeature;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Database.Entities;

namespace ChatApp.Features.ChatFeatures;

public static class CreateChatFeature
{
    public class CreateChatRequest
    {
        public required List<Guid> UsersIds { get; set; }
    }
    public class CreateChatResponse
    {
        public required ChatResponseDto ChatResponseDto{ get; set; }
    }
    //Nothing changed past here
    public class Validator : AbstractValidator<CreateChatRequest>
    {
        public Validator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(c => c.UsersIds.Count).GreaterThan(1);
            RuleFor(c => c.UsersIds.Count).LessThanOrEqualTo(10);
            RuleFor(c => c.UsersIds)
            .Must(uid => uid.Distinct().Count() == uid.Count)
            .WithMessage("List must not contain duplicate items... womp womp");
        }
    }

    public class Handler : IScoped
    {
        private readonly IChatService _chatService;
        private readonly IValidator<CreateChatRequest> _validator;
        
        public Handler(IValidator<CreateChatRequest> validator, IChatService chatService)
        {
            _chatService = chatService;
            _validator = validator;
        }
        public async Task<CreateChatResponse> Handle(CreateChatRequest request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);
            Chat chat = await _chatService.CreateChat(request.UsersIds);
            return new CreateChatResponse()
            {
                ChatResponseDto = new ChatResponseDto()
                {
                    Id = chat.Id
                }
            };
        }
    }
}

public class CreateChatEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/ChatFeatures/CreateChat", async ([FromBody] CreateChatRequest request, Handler handler, CancellationToken cancellationToken) =>
        {
            CreateChatResponse chatResponse = await handler.Handle(request, cancellationToken);
            return Results.Ok(chatResponse);
        })
        .WithTags("ChatFeatures")
        .WithMetadata(new AuthorizeAttribute { Roles = "User" })
        .RequireAuthorization();
    }
}
