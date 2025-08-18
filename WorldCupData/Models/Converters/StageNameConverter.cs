using DAL.Models.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DAL.Models.Converters;

internal class StageNameConverter : JsonConverter<StageName>
{
    public override bool CanConvert(Type t) => t == typeof(StageName);

    public override StageName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString()?.ToLowerInvariant();
        return value switch
        {
            "final" => StageName.Final,
            "first stage" => StageName.FirstStage,
            "play-off for third place" => StageName.PlayOffForThirdPlace,
            "match for third place" => StageName.PlayOffForThirdPlace,
            "quarter-finals" => StageName.QuarterFinals,
            "quarter-final" => StageName.QuarterFinals,
            "round of 16" => StageName.RoundOf16,
            "semi-finals" => StageName.SemiFinals,
            "semi-final" => StageName.SemiFinals,
            null => throw new JsonException($"{nameof(StageName)} cannot be null"),
            _ => throw new JsonException($"Unknown {nameof(StageName)} value: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, StageName value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case StageName.Final:
                JsonSerializer.Serialize(writer, "Final", options);
                return;
            case StageName.FirstStage:
                JsonSerializer.Serialize(writer, "First stage", options);
                return;
            case StageName.PlayOffForThirdPlace:
                JsonSerializer.Serialize(writer, "Play-off for third place", options);
                return;
            case StageName.QuarterFinals:
                JsonSerializer.Serialize(writer, "Quarter-finals", options);
                return;
            case StageName.RoundOf16:
                JsonSerializer.Serialize(writer, "Round of 16", options);
                return;
            case StageName.SemiFinals:
                JsonSerializer.Serialize(writer, "Semi-finals", options);
                return;
        }

        throw new Exception("Cannot marshal type StageName");
    }
}
