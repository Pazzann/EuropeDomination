using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;

public class Regiment
{
    public string Name;
    public int Cost;
    public int TemplateId;

    public int TimeFromStartOfTheTraining;
    public int TrainingTime;
    public bool IsFinished;

    public int Manpower; 
    public int MaxManpower;

    public BehavioralPatterns BehavioralPattern;
    public float Morale;
    public double[] Resources;
    public float CombatAbility;
    public float Defense;
    public float MovementSpeed;
    public float SurvivalIndex;

    public Modifiers Modifiers;
    
    
    public Regiment(string name, int cost, int templateId, int timeFromStartOfTheTraining, int trainingTime, bool isFinished, int manpower, int maxManpower)
    {
        Name = name;
        Cost = cost;
        TemplateId = templateId;
        
        
        TimeFromStartOfTheTraining = timeFromStartOfTheTraining;
        TrainingTime = trainingTime;
        IsFinished = isFinished;

        Manpower = manpower;
        MaxManpower = maxManpower;
        
    }


    public float CombatReadiness
    {
        //should be calculated by resource amount needed and in availability (max 1f)
        get => 1f;
    }
    
    public void Consume()
    {
        //consumes resources in their rate
    }
    public void Recalculate()
    {
        //after creation calculating stats via modifiers
    }
    
    public void ChangeTemplate()
    {
        // change template and things after it
    }

    public void DayTick()
    {
        
    }
}