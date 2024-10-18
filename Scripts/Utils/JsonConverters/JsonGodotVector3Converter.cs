using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

namespace EuropeDominationDemo.Scripts.Utils.JsonConverters;

public class JsonGodotVector3Converter : JsonConverter<Vector3>
{
    public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }
            
            var vec = new Vector3();

            string? propertyName = reader.GetString();
            if (propertyName != "X")
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }
            vec.X = (float)reader.GetDouble();
            propertyName = reader.GetString();
            if (propertyName != "Y")
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }
            
            vec.Y = (float)reader.GetDouble();
            
            propertyName = reader.GetString();
            if (propertyName != "Z")
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }
            vec.Z = (float)reader.GetDouble();

            return vec;

    }

    
    public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("X", value.X);
        writer.WriteNumber("Y", value.Y);
        writer.WriteNumber("Z", value.Z);
        writer.WriteEndObject();
    }
}