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

builder.Services.AddRealtimeDI();
builder.Services.AddApplicationDI();

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

    // Intentamos generar el documento Swagger al arrancar para atrapar errores de generación
    try
    {
        var swaggerProvider = app.Services.GetRequiredService<ISwaggerProvider>();
        // Esto forzará la creación del documento y fallará ahora en vez de cuando se llame a /swagger/v1/swagger.json
        var doc = swaggerProvider.GetSwagger("v1");
    }
    catch (Exception ex)
    {
        // Imprimir en consola para que puedas ver la traza completa al ejecutar la app
        Console.WriteLine("Error generating Swagger document at startup:");
        Console.WriteLine(ex.ToString());
        // No rethrow: dejamos que la app arranque para que puedas inspeccionar otros endpoints.
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

app.UseMiddleware<AuthMiddleware>();

app.MapControllers();

app.MapHub<RealtimeHub>("/realtime");

app.Run();