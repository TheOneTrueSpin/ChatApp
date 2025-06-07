using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carter;
using ChatApp.Database.Entities;
using ChatApp.Features.UserFeatures.Dtos;
using ChatApp.Shared.Services.UserServices;
using ChatApp.Shared.Utils;
using Microsoft.AspNetCore.Authorization;
using static ChatApp.Features.UserFeatures.GetUserByEmailFeature;

namespace ChatApp.Features.UserFeatures;

public static class GetUserByEmailFeature
{
    public class GetUserByEmailResponse
    {
        public required UserResponseDto UserResponseDto { get; set; }
    }
    public class Handler : IScoped
    {
        private readonly IUserService _userService;
        public Handler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<GetUserByEmailResponse> Handle(string email, CancellationToken cancellationToken)
        {
            UserResponseDto userResponse = await _userService.GetUserByEmail(email);
            return new GetUserByEmailResponse() { UserResponseDto = userResponse };
        }
    }
}
public class GetUserByEmailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/UserFeatures/GetUserByEmail", async (string email, Handler handler, CancellationToken cancellationToken) =>
        {
            GetUserByEmailResponse response = await handler.Handle(email, cancellationToken);
            return Results.Ok(response);
        })
        .WithTags("UserFeature")
        .WithMetadata(new AuthorizeAttribute { Roles = "User" })
        .RequireAuthorization();
    }
}
