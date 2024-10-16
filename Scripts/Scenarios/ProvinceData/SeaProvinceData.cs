namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public class SeaProvinceData : ProvinceData
{
    public Modifiers Modifiers { get; set; }

    public SeaProvinceData(
        int id,
        string name,
        Modifiers modifiers) : base(id, name)
    {
        Modifiers = modifiers;
    }
}