using System.Text.Json;

namespace DAL.Models.Converters;

internal static class Converter
{
    public static readonly JsonSerializerOptions Settings = new(JsonSerializerDefaults.General)
                                                            {
                                                                Converters =
                                                                {
                                                                    TypeOfEventConverter.Singleton,
                                                                    PositionConverter.Singleton,
                                                                    TacticsConverter.Singleton,
                                                                    new StageNameConverter(),
                                                                    StatusConverter.Singleton,
                                                                    TimeConverter.Singleton,
                                                                    DescriptionConverter.Singleton,
                                                                    new DateOnlyConverter(),
                                                                    new TimeOnlyConverter(),
                                                                    IsoDateTimeOffsetConverter.Singleton
                                                                },
                                                            };
}