namespace EuropeDominationDemo.Scripts.Scenarios.Buildings;

public class BuildingBonuses
{
    public float ProductionEfficiency { get; }
    public float TransportationCapacity { get; }

    public BuildingBonuses(float productionEfficiency, float transportationCapacity)
    {
        this.ProductionEfficiency = productionEfficiency;
        this.TransportationCapacity = transportationCapacity;
    }
}