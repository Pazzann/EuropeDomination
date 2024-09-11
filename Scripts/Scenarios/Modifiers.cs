namespace EuropeDominationDemo.Scripts.Scenarios;

public class Modifiers
{
    public float ProductionEfficiency { get; set; }
    public float TransportationBonus { get; set; }


    public float AdditionalTrainingEfficiency { get; set; }

    public float MaxMoraleBonus { get; set; }
    public float MaxMoraleEfficiency { get; set; }
    public float MoraleIncreaseEfficiency { get; set; }
    public float MaxManpowerBonus {get; set;}
    public float MaxManpowerEfficiency { get; set; }
    public float ManpowerIncreaseEfficiency { get; set; }

    public Modifiers(float productionEfficiency, float transportationBonus, float additionalTrainingEfficiency,
        float maxMoraleBonus, float maxMoraleEfficiency, float moraleIncreaseEfficiency, int maxManpowerBonus, float maxManpowerEfficiency, float manpowerIncreaseEfficiency)
    {
        ProductionEfficiency = productionEfficiency;
        TransportationBonus = transportationBonus;
        AdditionalTrainingEfficiency = additionalTrainingEfficiency;
        MaxMoraleBonus = maxMoraleBonus;
        MaxMoraleEfficiency = maxMoraleEfficiency;
        MoraleIncreaseEfficiency = moraleIncreaseEfficiency;
        MaxManpowerBonus = maxManpowerBonus;
        MaxManpowerEfficiency = maxManpowerEfficiency;
        ManpowerIncreaseEfficiency = manpowerIncreaseEfficiency;
    }


    public static Modifiers DefaultModifiers(float productionEfficiency = 1.0f, float transportationCapacity = 0.0f,
        float additionalTrainingEfficiency = 1.0f, float maxMoraleBonus = 0.0f, float maxMoraleEfficiency = 1.0f, float moraleIncreaseEfficiency = 1.0f,
        int maxManpowerBonus = 0, float maxManpowerEfficiency = 1.0f, float manpowerIncreaseEfficiency = 1.0f)
    {
        return new Modifiers(productionEfficiency, transportationCapacity, additionalTrainingEfficiency,
            maxMoraleBonus, maxMoraleEfficiency, moraleIncreaseEfficiency, maxManpowerBonus, maxManpowerEfficiency, manpowerIncreaseEfficiency);
    }
}