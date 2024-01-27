using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public abstract class Scenario : IScenario
{
	public abstract Dictionary<int, CountryData> Countries { get; }

	public abstract ProvinceData[] Map { get; set; }
	public abstract DateTime Date { get; set; }
	
	public abstract List<ArmyUnitData> ArmyUnits { get; set; }
	
	public abstract Image MapTexture { get; set; }

	public TimeSpan Ts => new(1, 0, 0, 0);

	public ProvinceData[] CountryProvinces(int countryId)
	{
		return Map.Where(t => countryId == t.Owner).ToArray();
	}
}
