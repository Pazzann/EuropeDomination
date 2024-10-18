using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using Steamworks;

namespace EuropeDominationDemo.Scripts.Utils.JsonConverters;

public class JsonSteamIdConverter: JsonConverter<SteamId>
{
    public override SteamId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (SteamId)reader.GetUInt64()!;
    }

    public override void Write(Utf8JsonWriter writer, SteamId value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue((ulong)value);
    }
}