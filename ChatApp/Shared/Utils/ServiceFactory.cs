using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ChatApp.Shared.Exceptions;

namespace ChatApp.Shared.Utils;

public class ServiceFactory<T> where T : class
{
    private readonly IEnumerable<T> _services;
    public ServiceFactory(IEnumerable<T> services)
    {
        _services = services;
    }

    public T GetService(Type type)
    {
        T? service = _services.FirstOrDefault(s => s.GetType() == type);

        if (service is null)
        {
            throw new ApiException("Service not found", HttpStatusCode.InternalServerError, ErrorCode.ServiceNotFound);
        }

        return service;
    }

    public T GetService(string type)
    {
        T? service = _services.FirstOrDefault(s => s.GetType().ToString().Split(".").Last() == type);

        if (service is null)
        {
            throw new ApiException("Service not found", HttpStatusCode.InternalServerError, ErrorCode.ServiceNotFound);
        }

        return service;
    }
}