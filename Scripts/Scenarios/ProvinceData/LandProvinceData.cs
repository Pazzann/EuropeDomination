using System.Collections.Generic;
using System.Transactions;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public class LandProvinceData : ProvinceData
{
	public int Owner;
	public int Development;

	public double[] Resources;
	public List<Building> Buildings;
	public SpecialBuilding[] SpecialBuildings;

	public readonly Terrain Terrain;
	public readonly Good Good;

	public TransportationRoute HarvestedTransport = null;

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
		double[] resources,
		List<Building> buildings,
		Modifiers modifiers,
		SpecialBuilding[] specialBuildings,
		TransportationRoute harvestedTransport
		) : base(id, name)
	{
		Owner = countryId;
		Terrain = terrain;
		Development = development;

		Resources = resources;
		Buildings = buildings;

		BorderderingProvinces = new int[] { };
		Good = good;

		Modifiers = modifiers;
		SpecialBuildings = specialBuildings;

		HarvestedTransport = harvestedTransport;

	}

	public void SetRoute(TransportationRoute harvestedRoute)
	{
		HarvestedTransport = harvestedRoute;
	}
}
