using System.Net;
using ChatApp.Shared.Exceptions;
using ChatApp.Startup;
using Carter;
using dotenv.net;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

builder.Services.ConfigureDataProtection();

builder.Services.AutomaticallyRegisterServices();

builder.Services.AutomaticallyRegisterScopedServices();

builder.Services.AddControllers();

builder.Services.RegisterDependencies();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

builder.Services.RegisterSwagger();

builder.Services.AddCarter();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.RegisterDatabase();

builder.Services.AddGlobalErrorHandling();

builder.Services.AddFirebase();

builder.Services.RegisterAuthentication(builder.Configuration);

var app = builder.Build();

app.UseSwaggerSetup();

app.UseGlobalErrorHandling();

app.MapCarter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.RunMigrations();

app.Run();
