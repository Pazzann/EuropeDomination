namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

public class Ship : Regiment
{
    public Ship(string name, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, int maxManpower) : base(name,  templateId, timeFromStartOfTheTraining,
        trainingTime, isFinished, manpower, maxManpower)
    {
    }
}