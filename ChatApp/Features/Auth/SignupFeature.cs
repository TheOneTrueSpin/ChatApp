using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ChatApp.Database;
using ChatApp.Database.Entities;
using ChatApp.Shared.Exceptions;
using ChatApp.Shared.Services.Auth;
using ChatApp.Shared.Services.Auth.IdentityProviders;
using ChatApp.Shared.Services.Auth.Entities;
using ChatApp.Shared.Utils;
using ChatApp.Shared.Utils.UnitOfWork;
using FluentValidation;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ChatApp.Features.Auth.SignupFeature;
using System.Net;
using ChatApp.Shared.Repositories.Interfaces;
namespace ChatApp.Features.Auth;

public static class SignupFeature
{
    public class SignupResult
    {
        public required string JwtToken { get; set; }
    }

    public class SignupRequest
    {
        public string AuthToken { get; set; } = string.Empty;
    }

    public class Validator : AbstractValidator<SignupRequest>
    {
        public Validator()
        {
            RuleFor(c => c.AuthToken).NotEmpty();
        }
    }

    public class Handler : IScoped
    {
        private readonly IValidator<SignupRequest> _validator;
        private readonly IIdentityProviderService _identityProviderService;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public Handler(IValidator<SignupRequest> validator, IIdentityProviderService identityProviderService, IAuthService authService, IUnitOfWork unitOfWork, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _validator = validator;
            _identityProviderService = identityProviderService;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<SignupResult> Handle(SignupRequest request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            AuthTokenClaims claims = await _identityProviderService.VerifyAuthToken(request.AuthToken);

            User? user = await _userRepository.GetUserByIdentityProviderUIdWithRoles(claims.UserId);

            if (user is not null)
            {
                throw new ApiException("User already registered.", HttpStatusCode.BadRequest, ErrorCode.UserAlreadyRegistered);
            }

            List<Role>? roles = await _roleRepository.GetRolesForNewUser();

            if (roles is null)
            {
                throw new Exception("Roles is null.");
            }

            if (roles.Count == 0)
            {
                throw new Exception("Roles are missing.");
            }

            User newUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = claims.Email,
                IdentityProviderUId = claims.UserId,
                Roles = roles
            };

            _userRepository.Add(newUser);

            await _unitOfWork.SaveChangesAsync();

            string jwtToken = _authService.GenerateJWTToken(newUser);
            await _authService.GenerateRefreshToken(newUser);

            return new SignupResult()
            {
                JwtToken = jwtToken
            };
        }
    }
}

public class SignupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/signup", async ([FromBody] SignupRequest command, Handler handler, CancellationToken cancellationToken) =>
        {
            SignupResult result = await handler.Handle(command, cancellationToken);

            return Results.Ok(result);
        })
        .WithTags("Auth");
    }
}
