using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Shared.Exceptions;

public static class ExceptionInfo
{
    private static readonly Dictionary<ErrorCode, string> ErrorMessages = new Dictionary<ErrorCode, string>()
    {
        // UnspecifiedError
        { ErrorCode.UnspecifiedError, "Error is unspecified" },

        // User Related Errors: 1000-1999
        { ErrorCode.UserNotFound, "The user does not exist" },
        { ErrorCode.UserNotRegistered, "The user has not been registered to the API yet. Register the user by calling the signup endpoint" },
        { ErrorCode.UserAlreadyRegistered, "The user has already registered an account" },

        // Auth Related Errors: 2000-2999
        { ErrorCode.InvalidAuthToken, "Auth token is invalid" },
        { ErrorCode.ExpiredAuthToken, "Auth token is expired" },
        { ErrorCode.RevokedAuthToken, "Auth token is revoked" },
        { ErrorCode.InvalidRefreshToken, "Refresh token is invalid" },

        // Service Related Errors: 3000-3999
        { ErrorCode.ServiceNotFound, "Service not found due to it not existing or not being registered" },

        // Miscellaneous Errors: 4000-4999
        { ErrorCode.ValidationError, "Validation Error" },
    };

    public static string GetErrorMessage(ErrorCode errorCode)
    {
        string? errorMessage = ErrorMessages.GetValueOrDefault(errorCode);

        if (errorMessage is null)
        {
            return "Error message missing for this error code.";
        }

        return errorMessage;
    }
}
