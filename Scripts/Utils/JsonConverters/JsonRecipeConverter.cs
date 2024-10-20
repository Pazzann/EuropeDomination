using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

namespace EuropeDominationDemo.Scripts.Utils.JsonConverters;

public class JsonRecipeConverter : JsonConverter<Recipe>
{
    public override Recipe Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();


        int id;
        int output;
        double outputAmount;
        var ingredients = new Dictionary<int, double>();
        

        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException();
        string? propertyName = reader.GetString();
        if (propertyName != "Id")
            throw new JsonException();

        reader.Read();
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException();
        id = reader.GetInt32();
        
        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException();
        propertyName = reader.GetString();
        if (propertyName != "Output")
            throw new JsonException();

        reader.Read();
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException();
        output = reader.GetInt32();
        
        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException();
        propertyName = reader.GetString();
        if (propertyName != "OutputAmount")
            throw new JsonException();

        reader.Read();
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException();
        outputAmount = reader.GetDouble();

        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException();
        propertyName = reader.GetString();
        if (propertyName != "Ingredients")
            throw new JsonException();

        reader.Read();
        if(reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();
        
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            var ingredient = reader.GetInt32();
            reader.Read();
            var amount = reader.GetDouble();
            ingredients.Add(ingredient, amount);
        }
        
        reader.Read();
        if(reader.TokenType!=JsonTokenType.EndObject)
            throw new JsonException();

        return new Recipe(id, ingredients, output, (float)outputAmount);
    }

    public override void Write(Utf8JsonWriter writer, Recipe value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        writer.WriteNumber("Id", value.Id);
        writer.WriteNumber("Output", value.Output);
        writer.WriteNumber("OutputAmount", value.OutputAmount);
        
        writer.WriteStartArray("Ingredients");
        foreach (var ingredient in value.Ingredients)
        {
            writer.WriteNumberValue(ingredient.Key);
            writer.WriteNumberValue(ingredient.Value);
        }
        writer.WriteEndArray();
        
        writer.WriteEndObject();
    }
}