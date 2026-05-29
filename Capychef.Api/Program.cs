using System.Text.Json;
using Capychef;
using Capychef.Api.Auth;
using Capychef.Api.Errors;
using Capychef.Api.Logging;
using Capychef.Api.Realtime;
using DotNetEnv;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

Env.Load("../.env");

// Add services to the container.

SerilogSetup.SetupSerilog(builder.Environment);

builder.Host.UseSerilog();

builder.Services.AddRealtimeDi();
builder.Services.AddApplicationDi();

var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")
    ?? throw new InvalidOperationException("REDIS_CONNECTION_STRING environment variable not found");

RateLimiterSetup.SetupRateLimiter(builder.Services, redisConnectionString);

builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalhostFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("LocalhostFrontend");

    try
    {
        var swaggerProvider = app.Services.GetRequiredService<ISwaggerProvider>();
        swaggerProvider.GetSwagger("v1");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error generating Swagger document at startup:");
        Console.WriteLine(ex.ToString());
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseRateLimiter();

app.UseMiddleware<AuthMiddleware>();

app.MapControllers();

app.MapHub<RealtimeHub>("/realtime");

app.Run();