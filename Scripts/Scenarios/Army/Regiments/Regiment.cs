using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;

public abstract class Regiment
{
    public string Name;
    public int Owner;
    public int TemplateId;

    public int TimeFromStartOfTheTraining;
    public int TrainingTime;
    public bool IsFinished;

    public int Manpower; 

    public float Morale;
    public double[] Resources;
    
    public BehavioralPatterns BehavioralPattern;    
    public Modifiers Modifiers;
    
    public Regiment(string name, int owner, int templateId, int timeFromStartOfTheTraining, int trainingTime, bool isFinished, int manpower, float morale, double[] resources, BehavioralPatterns behavioralPattern, Modifiers modifiers)
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
    public abstract float MoraleIncrease { get; }
    public abstract int ManpowerGrowth { get; }
    public abstract int MaxManpower { get; }
    public abstract float CombatReadiness { get; }
    public abstract void Consume();
    public abstract void Recalculate();
    public abstract void ChangeTemplate();
    public abstract float CombatAbility{ get; }
    public abstract float Defense{ get; }
    public abstract float MovementSpeed{ get; }
    public abstract float SurvivalIndex{ get; }

    public bool DayTick()
    {
        TimeFromStartOfTheTraining++;
        return TimeFromStartOfTheTraining >= TrainingTime;
    }
}