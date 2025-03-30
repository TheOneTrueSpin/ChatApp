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
using static ChatApp.Features.Example.ExampleFeature;

namespace ChatApp.Features.Example;

public static class ExampleFeature
{
    public class ExampleRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class Validator : AbstractValidator<ExampleRequest>
    {
        public Validator()
        {
            RuleFor(c => c.Title).NotEmpty();
        }
    }

    public class Handler : IScoped
    {
        private readonly IValidator<ExampleRequest> _validator;

        public Handler(IValidator<ExampleRequest> validator)
        {
            _validator = validator;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<Guid> Handle(ExampleRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _validator.ValidateAndThrow(request);

            throw new ApiException("test", HttpStatusCode.InternalServerError);

#pragma warning disable CS0162 // Unreachable code detected
            return Guid.NewGuid();
#pragma warning restore CS0162 // Unreachable code detected
        }
    }


}

public class CreateArticleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/example", async ([FromBody] ExampleRequest command, string content, Handler handler, CancellationToken cancellationToken) =>
        {
            command.Content = content;

            var articleId = await handler.Handle(command, cancellationToken);

            return Results.Ok(articleId);
        })
        .WithTags("Example");
        // .WithMetadata(new AuthorizeAttribute { Roles = "User,Admin" })
        // .RequireAuthorization();
    }
}