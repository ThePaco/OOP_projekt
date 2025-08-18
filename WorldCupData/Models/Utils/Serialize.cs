using DAL.Models.Converters;
using System.Text.Json;

namespace DAL.Models.Utils;

// no need to serialize since no matches will be added
//public static class Serialize
//{
//    public static string ToJson(this List<GroupResults> self) => JsonSerializer.Serialize(self, Converter.Settings);
//    public static string ToJson(this List<Matches> self) => JsonSerializer.Serialize(self, Converter.Settings);
//    public static string ToJson(this List<Results> self) => JsonSerializer.Serialize(self, Converter.Settings);
//    public static string ToJson(this List<Teams> self) => JsonSerializer.Serialize(self, Converter.Settings);
//}