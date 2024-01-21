using System;
using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;

public class DemoScenario : Scenario
{
	public override Dictionary<string, int> Countries { get; }
	public override Vector3[] CountriesColors { get; }
	public override string[] CountriesNames { get; }
	public override int ProvinceCount { get; }
	public override CountryData[] CountriesData { get; set; }
	public override ProvinceData[] Map { get; set; }
	public override DateTime Date { get; set; }
	public DemoScenario(Image mapTexture)
	{
		Date = new DateTime(1444, 11, 12);
		
		ProvinceCount = 14;
		Countries = new Dictionary<string, int>()
		{
			{ "Great Britian", 0 },
			{ "France", 1 },
			{ "Sweden", 2 }
		};
		CountriesColors = new Vector3[]
		{
			new Vector3(0.0f, 1.0f, 0.0f),
			new Vector3(0.0f, 0.0f, 1.0f),
			new Vector3(1.0f, 0.0f, 0.0f),
		};
		CountriesNames = new string[]
		{
			"Great Britian",
			"France",
			"Sweden"
		};
		CountriesData = new CountryData[]
		{
			new CountryData(Modifiers.DefaultModifiers()),
			new CountryData(Modifiers.DefaultModifiers()),
			new CountryData(Modifiers.DefaultModifiers())
		};
		Map = new ProvinceData[14]
		{
			new ProvinceData(0, Countries["Great Britian"], "Rekyavik", Terrain.Coast, Good.Iron, 1, new float[] {0, 2}, new List<Building>(), Modifiers.DefaultModifiers(), null),
			new ProvinceData(1,  Countries["Great Britian"], "Rekyavik", Terrain.Field, Good.Iron, 2, new float[] {0, 1}, new List<Building>(), Modifiers.DefaultModifiers(), new ArmyUnitData("test", Countries["Great Britian"], 1)),
			new ProvinceData(2,  Countries["Great Britian"], "Rekyavik", Terrain.Field, Good.Iron, 1, new float[] {2, 0}, new List<Building>(), Modifiers.DefaultModifiers(), null),
			new ProvinceData(3,  Countries["Great Britian"], "Rekyavik", Terrain.Forest, Good.Iron, 1, new float[] {0, 3}, new List<Building>(), Modifiers.DefaultModifiers(), null),
			new ProvinceData(4,  Countries["Great Britian"], "Rekyavik", Terrain.Plain, Good.Iron, 1, new float[] {4, 0}, new List<Building>(), Modifiers.DefaultModifiers(), null),
			new ProvinceData(5,  Countries["Great Britian"], "Rekyavik", Terrain.Plain, Good.Iron, 2, new float[] {0, 4}, new List<Building>(), Modifiers.DefaultModifiers(), null),
			new ProvinceData(6,  Countries["France"], "Rekyavik", Terrain.Plain, Good.Iron, 2, new float[] {3, 0}, new List<Building>(), Modifiers.DefaultModifiers(), null),
			new ProvinceData(7,  Countries["France"], "Rekyavik", Terrain.Plain, Good.Wheat, 1, new float[] {1, 0}, new List<Building>(), Modifiers.DefaultModifiers(), null),
			new ProvinceData(8,  Countries["France"], "Rekyavik", Terrain.Plain, Good.Wheat, 2, new float[] {0, 5}, new List<Building>(), Modifiers.DefaultModifiers(), null),
			new ProvinceData(9,  Countries["Sweden"], "Rekyavik", Terrain.Mountains, Good.Wheat, 1, new float[] {5, 0}, new List<Building>(), Modifiers.DefaultModifiers(), null),
			new ProvinceData(10, Countries["Sweden"], "Rekyavik", Terrain.Mountains, Good.Wheat, 1, new float[] {6, 0}, new List<Building>(), Modifiers.DefaultModifiers(), null),
			new ProvinceData(11, Countries["Sweden"], "Rekyavik", Terrain.Mountains, Good.Wheat, 1, new float[] {0, 6}, new List<Building>(), Modifiers.DefaultModifiers(), null),
			new ProvinceData(12,  Countries["Sweden"], "Rekyavik", Terrain.Coast, Good.Wheat, 1, new float[] {7, 0}, new List<Building>(), Modifiers.DefaultModifiers(), null),
			new ProvinceData(13, Countries["Sweden"], "Rekyavik", Terrain.Coast, Good.Wheat, 1, new float[] {0, 7}, new List<Building>(), Modifiers.DefaultModifiers(), null),
		};
		Map = GameMath.CalculateBorderProvinces(Map, mapTexture);
		var centers = GameMath.CalculateCenterOfProvinceWeight(mapTexture, ProvinceCount);
		for (int i = 0; i < ProvinceCount; i++)
		{
			Map[i].CenterOfWeight = centers[i];
		}
	}
}
