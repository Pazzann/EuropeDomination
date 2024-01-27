namespace EuropeDominationDemo.Scripts.Scenarios;

public class ArmyUnitData
{
    public string Name { get; set; }
    public int Owner { get; set; }
    public int CurrentProvince { get; set; }
    
    public Modifiers Modifiers { get; set; }

    

    public ArmyUnitData(string name, int owner, int currentProvince, Modifiers modifiers)
    {
        Name = name;
        Owner = owner;
        CurrentProvince = currentProvince;
        Modifiers = modifiers;
    }
}