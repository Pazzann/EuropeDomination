using System.Collections.Generic;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public abstract class Scenario : IScenario
{
    public abstract Dictionary<string, int> Countries { get;  }
    public abstract Vector3[] CountriesColors { get;  }
    public abstract string[] CountriesNames { get;  }
    public abstract int ProvinceCount { get; }
    public abstract CountryData[] CountriesData { get; set; }
    public abstract ProvinceData[] Map { get; set; }

    public ProvinceData[] CountryProvinces(int countryId)
    {
        List <ProvinceData> data = new List<ProvinceData>();

        for (int i = 0; i < ProvinceCount; i++)
        {
            if (countryId == Map[i].Owner)
            {
                data.Add(Map[i]);
            }
        }

        return data.ToArray();
    }
}