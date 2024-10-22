using System;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;


[Serializable]
public class LightShip : Ship
{
    public LightShip(string name, int owner, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, float morale, double[] resources, BehavioralPatterns behavioralPattern,
        Modifiers modifiers) : base(name, owner, templateId, timeFromStartOfTheTraining, trainingTime, isFinished,
        manpower,
        morale, resources, behavioralPattern, modifiers)
    {
    }
    [JsonConstructor]
    public LightShip()
    {
    }

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