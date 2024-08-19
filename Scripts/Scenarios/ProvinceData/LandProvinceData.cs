using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public class LandProvinceData : ProvinceData
{
	public readonly int Id;
	public readonly string Name;
	public int Owner;
	public int Development;

	public float[] Resources;
	public List<Building> Buildings;

	public readonly Terrain Terrain;
	public readonly Good Good;

	public Modifiers Modifiers;


	public float ProductionRate
	{
		get
		{
			var resourceProduced = 1.0f;
			foreach (var building in Buildings)
			{
				if (building.IsFinished)
					resourceProduced *= building.Modifiers.ProductionEfficiency;
			}

			return resourceProduced;
		}
	}

	public int UnlockedBuildingCount => 4 + Development / 5;


	public LandProvinceData(
		int id,
		int countryId,
		string name,
		Terrain terrain,
		Good good,
		int development,
		float[] resources,
		List<Building> buildings,
		Modifiers modifiers
	)
	{
		this.Id = id;
		this.Owner = countryId;
		this.Name = name;
		this.Terrain = terrain;
		this.Development = development;

		this.Resources = resources;
		this.Buildings = buildings;

		this.BorderderingProvinces = new int[] { };
		this.Good = good;

		this.Modifiers = modifiers;

	}
}
