using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Shared.Utils;

namespace ChatApp.Startup;

public static class AutomaticServiceRegistrationSetup
{
    public static IServiceCollection AutomaticallyRegisterServices(this IServiceCollection services)
    {
        var modules = typeof(Program).Assembly
            .GetTypes()
            .Where(t => typeof(IServiceModule).IsAssignableFrom(t) && !t.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IServiceModule>();

        foreach (var module in modules)
        {
            module.Register(services);
        }

        return services;
    }

    public static IServiceCollection AutomaticallyRegisterScopedServices(this IServiceCollection services)
    {
        var handlers = typeof(Program).Assembly
        .GetTypes()
        .Where(t => typeof(IScoped).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var handler in handlers)
        {
            services.AddScoped(handler);
        }

        return services;
    }
}
