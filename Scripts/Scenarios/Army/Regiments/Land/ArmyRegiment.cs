using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

public class ArmyRegiment : Regiment
{
    public int Manpower;
    public int MaxManpower;
    
    public Weapon Weapon;

    public ArmyRegiment(string name, int cost, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished) : base(name, cost, templateId, timeFromStartOfTheTraining, trainingTime, isFinished)
    {
        
    }
}