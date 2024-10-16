using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public class LandColonizedProvinceData : LandProvinceData
{
    public List<Building> Buildings;
    public int Development;

    public TransportationRoute HarvestedTransport;

    public Modifiers Modifiers;
    public int Owner;

    public double[] Resources;
    public SpecialBuilding[] SpecialBuildings;


    public LandColonizedProvinceData(
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
    ) : base(id, name, terrain, good)
    {
        Owner = countryId;
        Development = development;

        Resources = resources;
        Buildings = buildings;

        BorderderingProvinces = new int[] { };

        Modifiers = modifiers;
        SpecialBuildings = specialBuildings;

        HarvestedTransport = harvestedTransport;
    }

    public float ProductionRate => Buildings.Where(d => d.IsFinished)
        .Aggregate(EngineState.MapInfo.Scenario.Settings.InitialProduction, (acc, x) => acc * x.Modifiers.ProductionEfficiency);

    public int UnlockedBuildingCount => EngineState.MapInfo.Scenario.Settings.DevForCommonBuilding.Where(a => a <= Development).ToArray().Length;
    public float TaxIncome => Development * EngineState.MapInfo.Scenario.Settings.TaxEarningPerDev;
    public float ManpowerGrowth => Development * EngineState.MapInfo.Scenario.Settings.ManpowerPerDev;

    public void SetRoute(TransportationRoute harvestedRoute)
    {
        HarvestedTransport = harvestedRoute;
    }
}