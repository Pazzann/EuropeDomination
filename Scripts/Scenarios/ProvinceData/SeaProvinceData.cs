namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public class SeaProvinceData : ProvinceData
{
    public readonly int Id;
    public readonly string Name;
    public Modifiers Modifiers;

    public SeaProvinceData(
        int id,
        string name,
        Modifiers modifiers)
    {
        this.Id = id;
        this.Name = name;
        this.Modifiers = modifiers;
    }
        
}