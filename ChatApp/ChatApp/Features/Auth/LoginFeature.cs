using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ChatApp.Shared.Services.Auth.Entities;
using ChatApp.Shared.Services.Auth.IdentityProviders;
using Carter;
using FirebaseAdmin.Auth.Hash;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using static ChatApp.Features.Auth.LoginFeature;
using ChatApp.Database;
using Microsoft.EntityFrameworkCore;
using ChatApp.Database.Entities;
using ChatApp.Shared.Exceptions;
using System.Net;
using ChatApp.Shared.Services.Auth;
using ChatApp.Shared.Utils.UnitOfWork;
using ChatApp.Shared.Utils;
using ChatApp.Shared.Repositories.Interfaces;

namespace ChatApp.Features.Auth;

public static class LoginFeature
{
    public class LoginResult
    {
        public required string JwtToken { get; set; }
    }

    public class LoginRequest
    {
        public string AuthToken { get; set; } = string.Empty;
    }

    public class Validator : AbstractValidator<LoginRequest>
    {
        public Validator()
        {
            RuleFor(c => c.AuthToken).NotEmpty();
        }
    }

    public class Handler : IScoped
    {
        private readonly IValidator<LoginRequest> _validator;
        private readonly IIdentityProviderService _identityProviderService;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public Handler(IValidator<LoginRequest> validator, IIdentityProviderService identityProviderService, IAuthService authService, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _validator = validator;
            _identityProviderService = identityProviderService;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<LoginResult> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            AuthTokenClaims claims = await _identityProviderService.VerifyAuthToken(request.AuthToken);

            User? user = await _userRepository.GetUserByIdentityProviderUIdWithRoles(claims.UserId);

            if (user is null)
            {
                throw new ApiException("User not registered.", HttpStatusCode.BadRequest, ErrorCode.UserNotRegistered);
            }

            string jwtToken = _authService.GenerateJWTToken(user);
            await _authService.GenerateRefreshToken(user);

            return new LoginResult()
            {
                JwtToken = jwtToken
            };
        }
    }
}

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/login", async ([FromBody] LoginRequest request, Handler handler, CancellationToken cancellationToken) =>
        {
            LoginResult result = await handler.Handle(request, cancellationToken);

            return Results.Ok(result);
        })
        .WithTags("Auth");
    }
}