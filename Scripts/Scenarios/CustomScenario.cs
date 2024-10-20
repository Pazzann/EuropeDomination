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
    public override ScenarioSettings Settings { get; set; }
    public override Dictionary<int, Vector3> WastelandProvinceColors { get; set; }
    public override Vector3 WaterColor { get; set; }
    public override Vector3 UncolonizedColor { get; set; }
    public override Good[] Goods { get; }
    public override Recipe[] Recipes { get; set; }
    public override Terrain[] Terrains { get; }
    public override Building[] Buildings { get; }
    public override List<BattleData> Battles { get; set; }
    public override TechnologyTree[] TechnologyTrees { get; }
    public override Dictionary<int, CountryData> Countries { get; }
    public override ProvinceData.ProvinceData[] Map { get; set; }
    public override DateTime Date { get; set; }
    public override Dictionary<ulong, int> PlayerList { get; set; }

    [JsonConstructor]
    public CustomScenario()
    {
        
    }
}