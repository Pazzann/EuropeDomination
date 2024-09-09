using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

public class ArmyCavalryRegiment : ArmyRegiment
{
    public InfantryWeapon Weapon;
    public Horse Horse;
    public Helmet Helmet;
    public Armor Armor;
    public AdditionalSlotGood AdditionalSlot;
    public ArmyCavalryRegiment(string name, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, int maxManpower) : base(name,  templateId, timeFromStartOfTheTraining, trainingTime, isFinished, manpower, maxManpower)
    {
        
    }
}