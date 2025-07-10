using SensorApi.Models;

namespace SensorApi.Services;

public static class SensorConverter
{
    public static SensorResponse Convert(SensorInput input)
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
}