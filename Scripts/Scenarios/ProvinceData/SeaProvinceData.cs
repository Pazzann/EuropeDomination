namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public class SeaProvinceData : ProvinceData
{
    public readonly string Name;
    public Modifiers Modifiers;

    public SeaProvinceData(
        int id,
        string name,
        Modifiers modifiers) : base(id)
    {
        this.Name = name;
        this.Modifiers = modifiers;
    }
}