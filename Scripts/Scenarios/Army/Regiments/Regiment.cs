using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;

public abstract class Regiment
{
    public BehavioralPatterns BehavioralPattern;
    public bool IsFinished;

    public int Manpower;
    public Modifiers Modifiers;

    public float Morale;
    public string Name;
    public int Owner;
    public double[] Resources;
    public int TemplateId;

    public int TimeFromStartOfTheTraining;
    public int TrainingTime;

    public Regiment(string name, int owner, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, float morale, double[] resources, BehavioralPatterns behavioralPattern,
        Modifiers modifiers)
    {
        Name = name;
        Owner = owner;
        TemplateId = templateId;


        TimeFromStartOfTheTraining = timeFromStartOfTheTraining;
        TrainingTime = trainingTime;
        IsFinished = isFinished;

        Manpower = manpower;
        Morale = morale;
        Resources = resources;

        BehavioralPattern = behavioralPattern;
        Modifiers = modifiers;
    }


    public abstract float MaxMorale { get; }
    public abstract float CurrentMaxMorale { get; }
    public abstract float MoraleIncrease { get; }
    public abstract float CurrentMoraleIncrease { get; }
    public abstract int ManpowerGrowth { get; }
    public abstract int CurrentManpowerGrowth { get; }
    public abstract int MaxManpower { get; }
    public abstract int CurrentMaxManpower { get; }
    public abstract float Defense { get; }
    public abstract float MaxDefense { get; }
    public abstract float Attack { get; }
    public abstract float MaxAttack { get; }
    public abstract void Consume();
    public abstract void Recalculate();
    public abstract void ChangeTemplate();

    public bool DayTick()
    {
        TimeFromStartOfTheTraining++;
        return TimeFromStartOfTheTraining >= TrainingTime;
    }
}