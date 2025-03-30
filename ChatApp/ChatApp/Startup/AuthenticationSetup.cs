using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Startup;

public static class AuthenticationSetup
{
    public static IServiceCollection RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var publicRsaKey = RSA.Create();
            string pemPublicKey = Environment.GetEnvironmentVariable("JWT_RSA_KEY_PUBLIC") ?? throw new Exception("JWT_RSA_KEY_PUBLIC environment variable is null");
            publicRsaKey.ImportFromPem(pemPublicKey);

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                IssuerSigningKey = new RsaSecurityKey(publicRsaKey),
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"]
            };

        });

        return services;
    }
}
