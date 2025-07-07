using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

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
    var response = ConvertSensorData(input);
    logger.LogInformation($"受信: {input.SensorId}, 温度: {input.Temperature}");

    return Results.Ok(response);

    SensorResponse ConvertSensorData(SensorInput input)
    {
        var jstZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
        var jstTime = TimeZoneInfo.ConvertTimeFromUtc(input.Timestamp, jstZone);

        var response = new SensorResponse
        {
            Id = input.SensorId,
            Data = new Data
            {
                Temp_c = input.Temperature,
                Humidity_percent = input.Humidity
            },
            ReceivedAt = jstTime.ToString("yyyy-MM-ddTHH:mm:ssK")
        };

        if (input.Temperature > 100)
        {
            response.Warning = true;
        }

        return response;
    }
});

app.Run();

// ----------- モデルクラス -----------

class SensorInput
{
    [JsonPropertyName("sensorId")]
    public string SensorId { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}

class SensorResponse
{
    public string Id { get; set; }
    public Data Data { get; set; }
    public string ReceivedAt { get; set; }
    public bool? Warning { get; set; }
}

class Data
{
    public double Temp_c { get; set; }
    public int Humidity_percent { get; set; }
}