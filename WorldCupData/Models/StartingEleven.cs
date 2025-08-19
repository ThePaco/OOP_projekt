using DAL.Models.Converters;
using DAL.Models.Enums;
using System.Text.Json.Serialization;

namespace DAL.Models;

public class StartingEleven
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("captain")]
    public bool Captain { get; set; }

    [JsonPropertyName("shirt_number")]
    public long ShirtNumber { get; set; }

    [JsonPropertyName("position")]
    [JsonConverter(typeof(PositionConverter))]
    public Position Position { get; set; }
}