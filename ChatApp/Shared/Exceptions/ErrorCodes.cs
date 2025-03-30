using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Shared.Exceptions;

public enum ErrorCode
{
  // Unspecified Error
  UnspecifiedError = 0,

  // User Related Errors: 1000-1999
  UserNotFound = 1000,
  UserNotRegistered = 1001,
  UserAlreadyRegistered = 1002,

  // Auth Related Errors: 2000-2999
  InvalidAuthToken = 2000,
  ExpiredAuthToken = 2001,
  RevokedAuthToken = 2002,
  InvalidRefreshToken = 2003,

  // Service Related Errors: 3000-3999
  ServiceNotFound = 3000,

  // Miscellaneous Errors: 4000-4999
  ValidationError = 4000,
}
