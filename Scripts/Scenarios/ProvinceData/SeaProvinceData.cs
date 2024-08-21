namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public class SeaProvinceData : ProvinceData
{
    public Modifiers Modifiers;

    public SeaProvinceData(
        int id,
        string name,
        Modifiers modifiers) : base(id, name)
    {
        this.Modifiers = modifiers;
    }
}