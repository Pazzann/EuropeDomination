using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
[Serializable]
[JsonDerivedType(typeof(SeaProvinceData), typeDiscriminator: "seaProvinceData")]
[JsonDerivedType(typeof(UncolonizedProvinceData), typeDiscriminator: "uncolonizedProvinceData")]
[JsonDerivedType(typeof(LandColonizedProvinceData), typeDiscriminator: "landColonizedProvinceData")]
[JsonDerivedType(typeof(WastelandProvinceData), typeDiscriminator: "wastelandProvinceData")]
public abstract class ProvinceData
{
    public int Id { get; }
    public int[] BorderderingProvinces { get; set; }
    public Vector2 CenterOfWeight { get; set; }
    public string Name { get; set; }

    protected ProvinceData(int id, string name, int[] borderderingProvinces = null, Vector2 centerOfWeight = new Vector2())
    {
        Id = id;
        Name = name;
    }
}