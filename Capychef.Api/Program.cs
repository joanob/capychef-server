using System.Text.Json;
using Capychef;
using Capychef.Api.Logging;
using DotNetEnv;
using Serilog;
using YourOwnBoss.Common.Auth;
using YourOwnBoss.Common.Errors;

var builder = WebApplication.CreateBuilder(args);

Env.Load("../.env");

// Add services to the container.

SerilogSetup.SetupSerilog(builder.Environment);

builder.Host.UseSerilog();

builder.Services.AddApplicationDI();

builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseMiddleware<AuthMiddleware>();

app.MapControllers();

app.Run();