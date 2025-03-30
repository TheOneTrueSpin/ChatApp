using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Database.Entities;

public class User
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string IdentityProviderUId { get; set; }
    public List<RefreshToken>? RefreshTokens { get; set; }
    public List<Role>? Roles { get; set; }
}
