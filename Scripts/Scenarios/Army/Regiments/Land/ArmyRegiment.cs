using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

public abstract class ArmyRegiment : Regiment
{
    public ArmyRegiment(string name, int owner, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, float morale, double[] resources, BehavioralPatterns behavioralPattern,
        Modifiers modifiers) : base(name, owner, templateId, timeFromStartOfTheTraining, trainingTime, isFinished, manpower,
        morale, resources, behavioralPattern, modifiers)
    {
    }
}