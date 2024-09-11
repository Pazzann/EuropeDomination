namespace EuropeDominationDemo.Scripts.Scenarios.Technology;

public class Technology
{
    //etc


    public Technology(Modifiers modifiers, int initialCost, int researchTime, double[] resourcesRequired,
        int goodToUnlock = -1, int buildingToUnlock = -1, int recipyToUnlock = -1)
    {
        Modifiers = modifiers;
        InitialCost = initialCost;
        ResearchTime = researchTime;
        ResourcesRequired = resourcesRequired;
        GoodToUnlock = goodToUnlock;
        BuildingToUnlock = buildingToUnlock;
        RecipyToUnlock = recipyToUnlock;
    }

    public Modifiers Modifiers { get; }
    public int InitialCost { get; }
    public int ResearchTime { get; }
    public double[] ResourcesRequired { get; }
    public int GoodToUnlock { get; }
    public int BuildingToUnlock { get; }
    public int RecipyToUnlock { get; }
}