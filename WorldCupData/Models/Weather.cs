using DAL.Models.Converters;
using DAL.Models.Enums;
using System.Text.Json.Serialization;

namespace DAL.Models;

public class Weather
{
    [JsonPropertyName("humidity")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long Humidity { get; set; }

    [JsonPropertyName("temp_celsius")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long TempCelsius { get; set; }

    [JsonPropertyName("temp_farenheit")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long TempFarenheit { get; set; }

    [JsonPropertyName("wind_speed")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long WindSpeed { get; set; }

    [JsonPropertyName("description")]
    [JsonConverter(typeof(WeatherDescriptionConverter))]
    public WeatherDescription WeatherDescription { get; set; }
}