using DAL.Models.Converters;
using DAL.Models.Enums;
using System.Text.Json.Serialization;

namespace DAL.Models;

public class TeamEvent
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type_of_event")]
    [JsonConverter(typeof(TypeOfEventConverter))]
    public TypeOfEvent TypeOfEvent { get; set; }

    [JsonPropertyName("player")]
    public string Player { get; set; }

    [JsonPropertyName("time")]
    public string Time { get; set; }
}