namespace EuropeDominationDemo.Scripts.Scenarios.Buildings;

public class Empty : Building
{
    public override string Name => "Nothing Here";
    public override int TimeToBuild => 0;
    public override int Cost => 0;
    public override int ID => 0;

    public override BuildingBonuses Multipliers => new BuildingBonuses(1.0f, 1.0f);
}