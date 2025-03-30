using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database;
using ChatApp.Database.Entities;
using ChatApp.Shared.Repositories.Interfaces;
using ChatApp.Shared.Services.Auth;
using ChatApp.Shared.Services.Auth.Entities;
using ChatApp.Shared.Services.Auth.IdentityProviders;
using ChatApp.Shared.Utils;
using ChatApp.Shared.Utils.UnitOfWork;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using static ChatApp.Features.Auth.RefreshTokenFeature;

namespace ChatApp.Features.Auth;

public static class RefreshTokenFeature
{
    public class RefreshTokenResult
    {
        public required string JwtToken { get; set; }
    }

    public class RefreshTokenRequest
    {

    }

    public class Validator : AbstractValidator<RefreshTokenRequest>
    {
        public Validator()
        {

        }
    }

    public class Handler : IScoped
    {
        private readonly IValidator<RefreshTokenRequest> _validator;
        private readonly IIdentityProviderService _identityProviderService;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;

        public Handler(IValidator<RefreshTokenRequest> validator, IIdentityProviderService identityProviderService, DataContext dataContext, IAuthService authService, IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository)
        {
            _validator = validator;
            _identityProviderService = identityProviderService;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
        }

        public async Task<RefreshTokenResult> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            RefreshTokenCookie refreshTokenCookie = _authService.GetRefreshTokenFromCookie();

            RefreshToken refreshToken = await _authService.VerifyRefreshTokenAndThrow(refreshTokenCookie);

            User user = await _userRepository.GetUserByIdWithRoles(refreshToken.UserId) ?? throw new Exception("User does not exist with this refresh token id");

            string jwtToken = _authService.GenerateJWTToken(user);

            return new RefreshTokenResult()
            {
                JwtToken = jwtToken
            };
        }
    }


}

public class RefreshTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/refreshToken", async ([FromBody] RefreshTokenRequest command, Handler handler, CancellationToken cancellationToken) =>
        {
            RefreshTokenResult result = await handler.Handle(command, cancellationToken);

            return Results.Ok(result);
        })
        .WithTags("Auth");
    }
}
