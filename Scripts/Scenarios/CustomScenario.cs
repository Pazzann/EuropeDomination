using System;
using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using EuropeDominationDemo.Scripts.Scenarios.Technology;
using Godot;
using Newtonsoft.Json;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class CustomScenario : Scenario
{
    public override ScenarioSettings Settings { get; init; }
    public override Dictionary<int, Vector3> WastelandProvinceColors { get; init; }
    public override Vector3 WaterColor { get; init; }
    public override Vector3 UncolonizedColor { get; init; }
    public override Good[] Goods { get; init; }
    public override Recipe[] Recipes { get; init; }
    public override Terrain[] Terrains { get; init; }
    public override Building[] Buildings { get; init; }
    public override List<BattleData> Battles { get; init; }
    public override TechnologyTree[] TechnologyTrees { get; init; }
    public override Dictionary<int, CountryData> Countries { get; init; }
    public override ProvinceData.ProvinceData[] Map { get; init; }
    public override DateTime Date { get; set; }
    public override Dictionary<ulong, int> PlayerList { get; set; }

    [JsonConstructor]
    public CustomScenario()
    {
        
    }
}