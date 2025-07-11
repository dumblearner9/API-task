using System;
using Xunit;
using SensorApi.Models; // テスト対象のモデルを使用
using SensorApi.Services; // テスト対象のコンバーターを使用

namespace SensorApiMinimal.Tests.Services;

public class SensorConverterTests
{
    [Fact]
    public void Convert_StandardInput_ReturnsCorrectlyMappedResponse()
    {
        // Arrange (準備): テスト用の標準的な入力データを作成
        var input = new SensorInput
        {
            SensorId = "DEV-001",
            Temperature = 25.5,
            Humidity = 55,
            // UTCでタイムスタンプを指定。DateTimeKind.Utcが重要
            Timestamp = new DateTime(2024, 5, 20, 10, 30, 0, DateTimeKind.Utc) 
        };

        // Act (実行): テスト対象のメソッドを呼び出す
        var response = SensorConverter.Convert(input);

        // Assert (検証): 結果が期待通りかチェック
        Assert.NotNull(response);
        Assert.Equal("DEV-001", response.Id);
        
        Assert.NotNull(response.Data);
        Assert.Equal(25.5, response.Data.Temp_c);
        Assert.Equal(55, response.Data.Humidity_percent);
        
        // タイムゾーン変換の検証 (UTC 10:30 -> JST 19:30)
        // "K"フォーマット指定子はタイムゾーンオフセット(+09:00)を付与する
        Assert.Equal("2024-05-20T19:30:00+09:00", response.ReceivedAt);
        
        // 温度が100以下なのでWarningはセットされない(nullである)ことを確認
        Assert.Null(response.Warning);
    }

    [Fact]
    public void Convert_HighTemperatureInput_SetsWarningFlagToTrue()
    {
        // Arrange (準備): ユーザー提供のcurlの例に基づいたデータ
        var input = new SensorInput
        {
            SensorId = "ABC123",
            Temperature = 100.1, // 100を超える温度
            Humidity = 60,
            Timestamp = new DateTime(2025, 7, 1, 12, 0, 0, DateTimeKind.Utc)
        };

        // Act (実行)
        var response = SensorConverter.Convert(input);

        // Assert (検証)
        Assert.NotNull(response);
        Assert.Equal("ABC123", response.Id);
        
        // タイムゾーン変換の検証 (UTC 12:00 -> JST 21:00)
        Assert.Equal("2025-07-01T21:00:00+09:00", response.ReceivedAt);

        // Warningフラグがtrueになっていることを確認
        Assert.NotNull(response.Warning); // まずnullでないことを確認
        Assert.True(response.Warning);    // 次にtrueであることを確認
    }

    [Fact]
    public void Convert_BoundaryTemperatureInput_WarningFlagIsNull()
    {
        // Arrange (準備): 温度が境界値(100.0)のデータ
        var input = new SensorInput
        {
            SensorId = "BDR-456",
            Temperature = 100.0, // 境界値。"> 100"の条件は満たさない
            Humidity = 70,
            Timestamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        
        // Act (実行)
        var response = SensorConverter.Convert(input);

        // Assert (検証)
        Assert.NotNull(response);
        // Warningフラグがセットされていない(null)ことを確認
        Assert.Null(response.Warning);
    }
}