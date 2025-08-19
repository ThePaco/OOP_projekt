using System.Text.Json.Serialization;

namespace DAL.Models;

public class GroupResults
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("letter")]
    public string Letter { get; set; }

    [JsonPropertyName("ordered_teams")]
    public List<Results> OrderedTeams { get; set; }
}
