namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

public class MediumShipTemplate : ShipTemplate
{
    public MediumShipTemplate(string name, int id) : base(name, id)
    {
    }

    public override int TrainingTime { get; }

    public override float Cost => 7f;
}