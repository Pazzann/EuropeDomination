namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

public class Ship : Regiment
{
    public Ship(string name, int cost, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, int maxManpower) : base(name, cost, templateId, timeFromStartOfTheTraining,
        trainingTime, isFinished, manpower, maxManpower)
    {
    }
}