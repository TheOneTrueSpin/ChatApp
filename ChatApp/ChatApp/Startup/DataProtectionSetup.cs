using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;

namespace ChatApp.Startup;

public static class DataProtectionSetup
{
    public static IServiceCollection ConfigureDataProtection(this IServiceCollection services)
    {
        services.AddDataProtection()
            .UseEphemeralDataProtectionProvider();

        return services;
    }
}
