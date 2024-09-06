using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

public class ArmyArtilleryRegiment : ArmyRegiment
{
    public ArtilleryWeapon Weapon;
    public Boots Boots;
    public Armor Armor;
    public Wheel Wheel;
    public AdditionalSlotGood AdditionalSlot;
    public ArmyArtilleryRegiment(string name, int cost, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, int maxManpower) : base(name, cost, templateId, timeFromStartOfTheTraining, trainingTime, isFinished, manpower, maxManpower)
    {
        
    }
}