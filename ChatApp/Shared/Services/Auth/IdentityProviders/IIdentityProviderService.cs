using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Shared.Services.Auth.Entities;

namespace ChatApp.Shared.Services.Auth.IdentityProviders;

public interface IIdentityProviderService
{
    public Task<AuthTokenClaims> VerifyAuthToken(string authToken);
}
