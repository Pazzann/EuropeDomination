namespace EuropeDominationDemo.Scripts.Scenarios.Buildings;

public class Workshop : Building
{
    public override string Name => "Workshop";
    public override int TimeToBuild => 180;
    public override int Cost => 100;
    public override int ID => 1;
    public override BuildingBonuses Multipliers => new BuildingBonuses(1.5f, 1.0f);
}