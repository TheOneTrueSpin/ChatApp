using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Features.ChatFeatures;

public static class CreateChatFeature
{
    public class CreateChatRequest
    {
        public required MessageRequestDto MessageRequestDto {get;set;}
    }

    public class Validator : AbstractValidator<SendMessageRequest>
    {
        public Validator()
        {
            RuleFor(c => c.MessageRequestDto).NotNull();
        }
    }

    public class Handler : IScoped
    {
        private readonly IChatService _chatService;
        private readonly IValidator<SendMessageRequest> _validator;
        
        public Handler(IValidator<SendMessageRequest> validator, IChatService chatService)
        {
            _chatService = chatService;
            _validator = validator;
        }
        public async Task Handle(SendMessageRequest request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);
            await _chatService.SendMessage(request.MessageRequestDto);
        }
    }
}

public class SendMessageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/ChatFeatures/SendMessage", async ([FromBody] SendMessageRequest request, Handler handler, CancellationToken cancellationToken) =>
        {
            await handler.Handle(request, cancellationToken);
            return Results.Ok();
        })
        .WithTags("ChatFeatures")
        .WithMetadata(new AuthorizeAttribute { Roles = "User" })
        .RequireAuthorization();
    }
}
