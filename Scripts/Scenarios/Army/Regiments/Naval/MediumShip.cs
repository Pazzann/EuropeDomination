using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

public class MediumShip : Ship
{
    public MediumShip(string name, int owner, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, float morale, double[] resources, BehavioralPatterns behavioralPattern,
        Modifiers modifiers) : base(name, owner, templateId, timeFromStartOfTheTraining, trainingTime, isFinished,
        manpower,
        morale, resources, behavioralPattern, modifiers)
    {
    }

    public override float MaxMorale { get; }
    public override float CurrentMaxMorale { get; }
    public override float MoraleIncrease { get; }
    public override float CurrentMoraleIncrease { get; }
    public override int ManpowerGrowth { get; }
    public override int CurrentManpowerGrowth { get; }
    public override int MaxManpower { get; }
    public override int CurrentMaxManpower { get; }
    public override float Defense { get; }
    public override float MaxDefense { get; }
    public override float Attack { get; }
    public override float MaxAttack { get; }

    public override void Recalculate()
    {
    }

    public override void ChangeTemplate()
    {
    }

    public override void Consume()
    {
    }
}