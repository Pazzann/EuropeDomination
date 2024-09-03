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

    public void Recalculate()
    {
        //after creation calculating stats via modifiers
    }
    
    public void ChangeTemplate()
    {
        // change template and things after it
    }
}