using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Shared.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Shared.CrossCuttingConcerns.GlobalErrorHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();

        if (exception.GetType() == typeof(ApiException))
        {
            ApiException apiException = (ApiException)exception;

            problemDetails.Title = apiException.ErrorCode.ToString();
            problemDetails.Detail = ExceptionInfo.GetErrorMessage(apiException.ErrorCode);
            problemDetails.Status = (int)apiException.StatusCode;
            problemDetails.Extensions.Add("errorCode", apiException.ErrorCode);
            problemDetails.Extensions.Add("message", exception.Message);
            httpContext.Response.StatusCode = (int)apiException.StatusCode;
        }
        else if (exception.GetType() == typeof(ValidationException))
        {
            ValidationException validationException = (ValidationException)exception;

            problemDetails.Title = ErrorCode.ValidationError.ToString();
            problemDetails.Detail = ExceptionInfo.GetErrorMessage(ErrorCode.ValidationError);
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Extensions.Add("errorCode", ErrorCode.ValidationError);
            problemDetails.Extensions.Add("message", validationException.Message);
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
        else
        {
            problemDetails.Title = ErrorCode.UnspecifiedError.ToString();
            problemDetails.Detail = ExceptionInfo.GetErrorMessage(ErrorCode.UnspecifiedError);
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Extensions.Add("errorCode", ErrorCode.UnspecifiedError);
            problemDetails.Extensions.Add("message", exception.Message);
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
