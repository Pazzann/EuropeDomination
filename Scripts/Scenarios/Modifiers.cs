namespace EuropeDominationDemo.Scripts.Scenarios;

public class Modifiers
{
    public float ProductionEfficiency { get; set; }
    public float TransportationCapacity { get; set; }
    
    
    
    
    
    public float AdditionalTrainingEfficiency { get; set; }

    public Modifiers(float productionEfficiency, float transportationCapacity, float additionalTrainingEfficiency)
    {
        ProductionEfficiency = productionEfficiency;
        TransportationCapacity = transportationCapacity;
        AdditionalTrainingEfficiency = additionalTrainingEfficiency;
    }


    public static Modifiers DefaultModifiers(float productionEfficiency = 0.0f, float transportationCapacity= 0.0f, float additionalTrainingEfficiency = 1.0f)
    {
        return new Modifiers(productionEfficiency, transportationCapacity, additionalTrainingEfficiency);
    }
}