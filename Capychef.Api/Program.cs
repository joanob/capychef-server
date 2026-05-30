using System.Text.Json;
using Capychef;
using Capychef.Api;
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

RateLimiterSetup.SetupRateLimiter(builder.Services, builder.Environment);

CorsSetup.SetupCors(builder.Services);

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
if (app.Environment.IsDev())
{
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

app.UseCors(CorsSetup.PolicyName);

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseRateLimiter();

app.UseMiddleware<AuthMiddleware>();

app.MapControllers();

app.MapHub<RealtimeHub>("/realtime");

app.Run();