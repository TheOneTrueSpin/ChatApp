using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Shared.Services.Auth.Entities;

public class RefreshTokenCookie
{
    public required Guid Id { get; set; }
    public required string Token { get; set; }
    public required DateTime ExpiresOnUtc { get; set; }
}
