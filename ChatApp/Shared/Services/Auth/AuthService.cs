using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ChatApp.Database;
using ChatApp.Database.Entities;
using ChatApp.Shared.Exceptions;
using ChatApp.Shared.Repositories.Interfaces;
using ChatApp.Shared.Services.Auth.Entities;
using ChatApp.Shared.Utils;
using ChatApp.Shared.Utils.UnitOfWork;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Shared.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, DataContext dataContext, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public Guid GetUserId()
    {
        HttpContext httpContext = _httpContextAccessor.HttpContext ?? throw new Exception("GetUserId method used outside of a http request");

        Claim userIdClaim = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == "user_id") ?? throw new Exception("user_id claim does not exist in jwt token");

        Guid userId = Guid.Parse(userIdClaim.Value);

        return userId;

    }

    public string GenerateJWTToken(User user)
    {
        var privateRsaKey = RSA.Create();
        string pemPrivateKey = Environment.GetEnvironmentVariable("JWT_RSA_KEY_PRIVATE") ?? throw new Exception("JWT_RSA_KEY_PUBLIC environment variable is null");
        privateRsaKey.ImportFromPem(pemPrivateKey);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(privateRsaKey), SecurityAlgorithms.RsaSha256);

        var authClaims = new List<Claim>()
        {
            new Claim("user_id", user.Id.ToString()),

        };

        if (user.Roles is null)
        {
            throw new Exception("Roles is null");
        }

        foreach (var role in user.Roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = signingCredentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    public async Task GenerateRefreshToken(User user)
    {
        string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        RefreshToken refreshToken = new RefreshToken()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            RefreshTokenHash = CryptoUtil.ComputeSHA512Hash(token, 100),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshToken:ExpirationInDays"))
        };

        _refreshTokenRepository.Add(refreshToken);

        await _unitOfWork.SaveChangesAsync();

        SetRefreshTokenCookie(refreshToken, token);

    }

    public async Task<RefreshToken> VerifyRefreshTokenAndThrow(RefreshTokenCookie refreshTokenCookie)
    {
        RefreshToken? refreshToken = await _refreshTokenRepository.GetById(refreshTokenCookie.Id);

        if (refreshToken is null)
        {
            throw new ApiException("Refresh token with this id does not exist in database", HttpStatusCode.BadRequest, ErrorCode.InvalidRefreshToken);
        }

        // Check if refresh token is expired
        if (DateTime.UtcNow >= refreshToken.ExpiresOnUtc)
        {
            throw new ApiException("Refresh token has expired", HttpStatusCode.BadRequest, ErrorCode.InvalidRefreshToken);
        }

        // check if the token matches
        byte[] refreshTokenHash = refreshToken.RefreshTokenHash;
        byte[] refreshTokenCookieHash = CryptoUtil.ComputeSHA512Hash(refreshTokenCookie.Token, 100);
        if (!refreshTokenHash.SequenceEqual(refreshTokenCookieHash))
        {
            throw new ApiException("Refresh token does not match", HttpStatusCode.BadRequest, ErrorCode.InvalidRefreshToken);
        }

        return refreshToken;

    }

    public RefreshTokenCookie GetRefreshTokenFromCookie()
    {
        HttpContext httpContext = _httpContextAccessor.HttpContext ?? throw new Exception("Can't access http context.");

        string? refreshTokenBase64 = httpContext.Request.Cookies["RefreshToken"];

        if (refreshTokenBase64 is null)
        {
            throw new ApiException("Could not find refresh token cookie in request", HttpStatusCode.BadRequest, ErrorCode.InvalidRefreshToken);
        }

        if (refreshTokenBase64.Length > 1000)
        {
            throw new ApiException("Refresh token is too large", HttpStatusCode.BadRequest);
        }

        try
        {
            RefreshTokenCookie refreshTokenCookie = JsonSerializer.Deserialize<RefreshTokenCookie>(Encoding.UTF8.GetString(Convert.FromBase64String(refreshTokenBase64))) ?? throw new Exception("Invalid refresh token");
            return refreshTokenCookie;
        }
        catch
        {
            throw new ApiException("Format of refresh token cookie is invalid", HttpStatusCode.BadRequest, ErrorCode.InvalidRefreshToken);
        }

    }

    private void SetRefreshTokenCookie(RefreshToken refreshToken, string token)
    {
        RefreshTokenCookie refreshTokenCookie = new RefreshTokenCookie()
        {
            Id = refreshToken.Id,
            Token = token,
            ExpiresOnUtc = refreshToken.ExpiresOnUtc
        };

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = new DateTimeOffset(refreshToken.ExpiresOnUtc)
        };

        string refreshTokenCookieBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(refreshTokenCookie)));

        HttpContext httpContext = _httpContextAccessor.HttpContext ?? throw new Exception("Can't access http context.");

        httpContext.Response.Cookies.Append("RefreshToken", refreshTokenCookieBase64, cookieOptions);

    }
}
