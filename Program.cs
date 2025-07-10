using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SensorApi.Models;
using SensorApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "yyyy/MM/dd HH:mm:ss ";
});

var app = builder.Build();

app.MapPost("/api/convert", (SensorInput input, ILogger<Program> logger) =>
{
    var response = SensorConverter.Convert(input);
    logger.LogInformation($"受信: {input.SensorId}, 温度: {input.Temperature}");
    return Results.Ok(response);
});

app.Run();