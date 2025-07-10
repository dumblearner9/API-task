using System.Text.Json.Serialization;

namespace SensorApi.Models;

public class SensorInput
{
    [JsonPropertyName("sensorId")]
    public string SensorId { get; set; } = string.Empty;

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}