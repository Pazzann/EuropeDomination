using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Goods;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public class UncolonizedProvinceData : LandProvinceData
{
    public int SettlersCombined;
    public int SettlersNeeded;
    public CountryData CurrentlyColonizedByCountry;
    
    public Modifiers Modifiers;
    public UncolonizedProvinceData(int id, string name, Terrain terrain, Good good, Modifiers modifiers, int settlersCombined = 0, int settlersNeeded = 5000, CountryData currentlyColonizedByCountry = null) : base(id, name, terrain, good)
    {
        SettlersCombined = settlersCombined;
        SettlersNeeded = settlersNeeded;
        CurrentlyColonizedByCountry = currentlyColonizedByCountry;
        Modifiers = modifiers;
    }
}