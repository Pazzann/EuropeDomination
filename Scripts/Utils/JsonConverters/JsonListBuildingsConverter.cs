using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;

namespace EuropeDominationDemo.Scripts.Utils.JsonConverters;

public class JsonListBuildingsConverter: JsonConverter<List<Building>>
{
    public override List<Building> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(reader.TokenType!=JsonTokenType.StartArray)
            throw new JsonException();

        var buildings = new List<Building>();
        reader.Read();
        while (reader.TokenType != JsonTokenType.EndArray)
        {
            buildings.Add(JsonSerializer.Deserialize<Building>(ref reader)!);
            reader.Read();
        }

        return buildings;
    }

    public override void Write(Utf8JsonWriter writer, List<Building> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var building in value)
        {
            JsonSerializer.Serialize(writer, building);
        }
        writer.WriteEndArray();
    }
}