namespace EuropeDominationDemo.Scripts.Scenarios.Army;

public class UnitData
{
    public string Name { get; set; }
    public int Owner { get; set; }

    public int CurrentProvince { get; set; }
    
    public Modifiers Modifiers { get; set; }

    public UnitData(string name, int owner, int currentProvince, Modifiers modifiers)
    {
        Name = name;
        CurrentProvince = currentProvince;
        Modifiers = modifiers;
        Owner = owner;
    }
}