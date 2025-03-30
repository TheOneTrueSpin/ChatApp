using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ChatApp.Database;
using ChatApp.Shared.Utils;
using DbUp;
using DbUp.Engine;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Startup;

public static class DatabaseSetup
{
    public static IServiceCollection RegisterDatabase(this IServiceCollection services)
    {

        string? connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION_STRING");

        if (connectionString is null)
        {
            throw new Exception("POSTGRESQL_CONNECTION_STRING environment variables is null");
        }

        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    public static WebApplication RunMigrations(this WebApplication app)
    {

        if (app.Environment.IsDevelopment())
        {
            string? connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION_STRING");

            if (connectionString is null)
            {
                throw new Exception("POSTGRESQL_CONNECTION_STRING environment variable is null");
            }

            EnsureDatabase.For.PostgresqlDatabase(connectionString);

            var upgrader = DeployChanges.To.PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(typeof(DataContext).Assembly)
                .LogToConsole()
                .Build();

            if (upgrader.IsUpgradeRequired())
            {
                var result = upgrader.PerformUpgrade();

                Console.WriteLine("Performing upgrade");

                if (!result.Successful)
                {
                    throw new Exception("Failed to perform migration successfully");
                }
            }

        }

        return app;
    }
}
