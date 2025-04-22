using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Shared.Repositories;
using ChatApp.Shared.Repositories.Interfaces;
using ChatApp.Shared.Services.Auth;
using ChatApp.Shared.Services.Auth.IdentityProviders;
using ChatApp.Shared.Services.ChatServices;
using ChatApp.Shared.Utils.UnitOfWork;

namespace ChatApp.Startup;

public static class DependencySetup
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IIdentityProviderService, FirebaseAuthService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IChatService, ChatService>();

        // Utils
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }
}
