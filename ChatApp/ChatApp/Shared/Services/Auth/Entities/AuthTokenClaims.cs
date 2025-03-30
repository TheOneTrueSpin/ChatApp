using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Shared.Services.Auth.Entities
{
    public class AuthTokenClaims
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
    }
}