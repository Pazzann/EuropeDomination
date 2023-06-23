namespace EuropeDominationDemo.Scripts.Scenarios;

public class Modifiers
{
    public float ProductionEfficiency { get; set; }
    public float TransportationCapacity { get; set; }

    public Modifiers(float productionEfficiency, float transportationCapacity)
    {
        this.ProductionEfficiency = productionEfficiency;
        this.TransportationCapacity = transportationCapacity;
    }


    public static Modifiers DefaultModifiers()
    {
        return new Modifiers(0.0f, 0.0f);
    }
}