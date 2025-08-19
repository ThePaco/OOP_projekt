using DAL.Models.Converters;
using DAL.Models.Enums;
using System.Text.Json.Serialization;

namespace DAL.Models;

public class TeamStatistics
{
    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("attempts_on_goal")]
    public long AttemptsOnGoal { get; set; }

    [JsonPropertyName("on_target")]
    public long OnTarget { get; set; }

    [JsonPropertyName("off_target")]
    public long OffTarget { get; set; }

    [JsonPropertyName("blocked")]
    public long Blocked { get; set; }

    [JsonPropertyName("woodwork")]
    public long Woodwork { get; set; }

    [JsonPropertyName("corners")]
    public long Corners { get; set; }

    [JsonPropertyName("offsides")]
    public long Offsides { get; set; }

    [JsonPropertyName("ball_possession")]
    public long BallPossession { get; set; }

    [JsonPropertyName("pass_accuracy")]
    public long PassAccuracy { get; set; }

    [JsonPropertyName("num_passes")]
    public long NumPasses { get; set; }

    [JsonPropertyName("passes_completed")]
    public long PassesCompleted { get; set; }

    [JsonPropertyName("distance_covered")]
    public long DistanceCovered { get; set; }

    [JsonPropertyName("balls_recovered")]
    public long BallsRecovered { get; set; }

    [JsonPropertyName("tackles")]
    public long Tackles { get; set; }

    [JsonPropertyName("clearances")]
    public long? Clearances { get; set; }

    [JsonPropertyName("yellow_cards")]
    public long? YellowCards { get; set; }

    [JsonPropertyName("red_cards")]
    public long RedCards { get; set; }

    [JsonPropertyName("fouls_committed")]
    public long? FoulsCommitted { get; set; }

    [JsonPropertyName("tactics")]
    [JsonConverter(typeof(TacticsConverter))]
    public Tactics Tactics { get; set; }

    [JsonPropertyName("starting_eleven")]
    public List<StartingEleven> StartingEleven { get; set; }

    [JsonPropertyName("substitutes")]
    public List<StartingEleven> Substitutes { get; set; }
}