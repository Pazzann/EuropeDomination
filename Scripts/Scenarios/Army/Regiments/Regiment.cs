namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;

public class Regiment
{
    public string Name;
    public int Cost;
    public int TemplateId;

    public int TimeFromStartOfTheTraining;
    public int TrainingTime;
    public bool IsFinished;

    public Regiment(string name, int cost, int templateId, int timeFromStartOfTheTraining, int trainingTime, bool isFinished)
    {
        Name = name;
        Cost = cost;
        TemplateId = templateId;
        
        
        TimeFromStartOfTheTraining = timeFromStartOfTheTraining;
        TrainingTime = trainingTime;
        IsFinished = isFinished;
    }

    public void ChangeTemplate()
    {
        
    }
}