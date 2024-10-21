using System;
using System.Text.Json.Serialization;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
[Serializable]
public class SeaProvinceData : ProvinceData
{
    public Modifiers Modifiers { get; set; }

    public SeaProvinceData(
        int id,
        string name,
        Modifiers modifiers,
        int[] borderderingProvinces = null, 
        Vector2 centerOfWeight = new ()) : base(id, name, borderderingProvinces, centerOfWeight)
    {
        Modifiers = modifiers;
    }

    [JsonConstructor]
    public SeaProvinceData()
    {
        
    }
}