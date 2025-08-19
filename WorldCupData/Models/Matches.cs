using DAL.Models.Converters;
using DAL.Models.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DAL.Models;

public class Matches
{
    [JsonPropertyName("venue")]
    public string Venue { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(StatusConverter))]
    public Status Status { get; set; } 

    [JsonPropertyName("time")]
    [JsonConverter(typeof(TimeConverter))]
    public Time Time { get; set; }

    [JsonPropertyName("fifa_id")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long FifaId { get; set; }

    [JsonPropertyName("weather")]
    public Weather Weather { get; set; }

    [JsonPropertyName("attendance")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long Attendance { get; set; }

    [JsonPropertyName("officials")]
    public List<string> Officials { get; set; }

    [JsonPropertyName("stage_name")]
    [JsonConverter(typeof(StageNameConverter))]
    public StageName StageName { get; set; }

    [JsonPropertyName("home_team_country")]
    public string HomeTeamCountry { get; set; }

    [JsonPropertyName("away_team_country")]
    public string AwayTeamCountry { get; set; }

    [JsonPropertyName("datetime")]
    public DateTimeOffset Datetime { get; set; }

    [JsonPropertyName("winner")]
    public string Winner { get; set; }

    [JsonPropertyName("winner_code")]
    public string WinnerCode { get; set; }

    [JsonPropertyName("home_team")]
    public Team HomeTeam { get; set; }

    [JsonPropertyName("away_team")]
    public Team AwayTeam { get; set; }

    [JsonPropertyName("home_team_events")]
    public List<TeamEvent> HomeTeamEvents { get; set; }

    [JsonPropertyName("away_team_events")]
    public List<TeamEvent> AwayTeamEvents { get; set; }

    [JsonPropertyName("home_team_statistics")]
    public TeamStatistics HomeTeamStatistics { get; set; }

    [JsonPropertyName("away_team_statistics")]
    public TeamStatistics AwayTeamStatistics { get; set; }

    [JsonPropertyName("last_event_update_at")]
    public DateTimeOffset LastEventUpdateAt { get; set; }

    [JsonPropertyName("last_score_update_at")]
    public DateTimeOffset? LastScoreUpdateAt { get; set; }
}

