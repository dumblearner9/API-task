namespace SensorApi.Models;

public class SensorResponse
{
    public string Id { get; set; } = string.Empty;
    public required Data Data { get; set; } 
    public string ReceivedAt { get; set; } = string.Empty;
    public bool? Warning { get; set; }
}