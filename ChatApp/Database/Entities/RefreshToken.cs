using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Database.Entities;

public class RefreshToken
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required byte[] RefreshTokenHash { get; set; }
    public required DateTime ExpiresOnUtc { get; set; }
}
