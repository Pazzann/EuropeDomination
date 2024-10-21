using System;
using System.Text.Json.Serialization;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

[Serializable]
public class WastelandProvinceData : ProvinceData
{
    public WastelandProvinceData(int id, string name,
        int[] borderderingProvinces = null,
        Vector2 centerOfWeight = new()) : base(id, name, borderderingProvinces, centerOfWeight)
    {
        
        
    }
    
    [JsonConstructor]
    public WastelandProvinceData()
    {
        
    }
}