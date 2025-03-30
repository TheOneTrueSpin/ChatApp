using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database.Entities;
using ChatApp.Shared.Services.Auth.Entities;

namespace ChatApp.Shared.Services.Auth;

public interface IAuthService
{
    public string GenerateJWTToken(User user);
    public Task GenerateRefreshToken(User user);
    public Task<RefreshToken> VerifyRefreshTokenAndThrow(RefreshTokenCookie refreshTokenCookie);
    public RefreshTokenCookie GetRefreshTokenFromCookie();
}
