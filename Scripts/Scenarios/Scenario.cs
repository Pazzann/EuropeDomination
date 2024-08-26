using System;
using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public abstract class Scenario : IScenario
{
	public abstract Dictionary<int, CountryData> Countries { get; }
	
	public abstract ProvinceData.ProvinceData[] Map { get; set; }
	public abstract Dictionary<int, Vector3> WastelandProvinceColors { get; set; }
	public abstract Vector3 WaterColor { get; set; }
	public abstract Vector3 UncolonizedColor { get; set; }
	public abstract DateTime Date { get; set; }
	
	public abstract List<ArmyUnitData> ArmyUnits { get; set; }
	
	public abstract Image MapTexture { get; set; }
	

	public TimeSpan Ts => new(1, 0, 0, 0);

	public ProvinceData.ProvinceData[] CountryProvinces(int countryId)
	{
		return Map.Where(t  => t is LandProvinceData data && countryId == data.Owner).ToArray();
	}
}
