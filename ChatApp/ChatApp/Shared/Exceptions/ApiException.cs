using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ChatApp.Shared.Exceptions;

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; init; }
    public ErrorCode ErrorCode { get; init; }
    public ApiException(string message, HttpStatusCode statusCode, ErrorCode? errorCode = null) : base(message)
    {
        StatusCode = statusCode;
        if (errorCode is null)
        {
            ErrorCode = ErrorCode.UnspecifiedError;
        }
        else
        {
            ErrorCode = (ErrorCode)errorCode;
        }
    }

    public ApiException(string message, HttpStatusCode statusCode, Exception? innerException, ErrorCode? errorCode = null) : base(message, innerException)
    {
        StatusCode = statusCode;
        if (errorCode is null)
        {
            ErrorCode = ErrorCode.UnspecifiedError;
        }
        else
        {
            ErrorCode = (ErrorCode)errorCode;
        }

    }

    public override string ToString()
    {
        string statusCodeName;
        if (Enum.IsDefined(typeof(HttpStatusCode), StatusCode))
        {
            statusCodeName = StatusCode.ToString();
        }
        else
        {
            statusCodeName = "Unknown Status Code";
        }

        return $"{base.ToString()}, ErrorCode: {(int)ErrorCode}, ErrorCodeMessage: {ExceptionInfo.GetErrorMessage(ErrorCode)}, HttpStatusCode: {(int)StatusCode} {statusCodeName}";
    }
}
