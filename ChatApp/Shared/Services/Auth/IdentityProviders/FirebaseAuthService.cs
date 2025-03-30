using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ChatApp.Shared.Exceptions;
using ChatApp.Shared.Services.Auth.Entities;
using ChatApp.Shared.Services.Auth.IdentityProviders;
using FirebaseAdmin.Auth;

namespace ChatApp.Shared.Services.Auth.IdentityProviders;

public class FirebaseAuthService : IIdentityProviderService
{
    public async Task<AuthTokenClaims> VerifyAuthToken(string authToken)
    {
        try
        {
            var token = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(authToken);

            object userIdObj = token.Claims.GetValueOrDefault("user_id") ?? throw new Exception("user_id firebase claim missing for id token");
            string userId = userIdObj.ToString() ?? throw new Exception("Failed to convert user_id firebase claim object to string");

            object emailObj = token.Claims.GetValueOrDefault("email") ?? throw new Exception("email firebase claim missing for id token");
            string email = emailObj.ToString() ?? throw new Exception("Failed to convert email firebase claim object to string");

            AuthTokenClaims authTokenClaims = new AuthTokenClaims()
            {
                UserId = userId,
                Email = email,
            };

            return authTokenClaims;
        }
        catch (FirebaseAuthException ex) when (ex.AuthErrorCode == AuthErrorCode.InvalidIdToken)
        {
            throw new ApiException(ex.Message, HttpStatusCode.Unauthorized, ErrorCode.InvalidAuthToken);
        }
        catch (FirebaseAuthException ex) when (ex.AuthErrorCode == AuthErrorCode.ExpiredIdToken)
        {
            throw new ApiException(ex.Message, HttpStatusCode.Unauthorized, ErrorCode.ExpiredAuthToken);
        }
        catch (FirebaseAuthException ex) when (ex.AuthErrorCode == AuthErrorCode.RevokedIdToken)
        {
            throw new ApiException(ex.Message, HttpStatusCode.Unauthorized, ErrorCode.RevokedAuthToken);
        }

    }
}
