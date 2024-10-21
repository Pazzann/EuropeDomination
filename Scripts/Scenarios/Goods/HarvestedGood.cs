using System;
using System.Text.Json.Serialization;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods;

[Serializable]
public class HarvestedGood : Good
{
    public HarvestedGood(int id, string name, Vector3 color, float cost) : base(id, name, color, cost)
    {
    }
    [JsonConstructor]
    public HarvestedGood()
    {
        
    }
}