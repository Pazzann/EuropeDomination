namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

public class LightShipTemplate : ShipTemplate
{
    public LightShipTemplate(string name, int id) : base(name, id)
    {
        
    }
    
    public override int TrainingTime { get; }
}