using System;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
[Serializable]
public abstract class LandProvinceData : ProvinceData
{
    public int Good { get; set; }
    public int Terrain { get; set; }

    public LandProvinceData(int id, string name, int terrain, int good,int[] borderderingProvinces = null, 
        Vector2 centerOfWeight = new ()) : base(id, name, borderderingProvinces, centerOfWeight)
    {
        Terrain = terrain;
        Good = good;
    }
    [JsonConstructor]
    public LandProvinceData()
    {
        
    }
}