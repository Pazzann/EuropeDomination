using System;
using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.UI.Events.ToEngine;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

[Serializable]
public class MilitaryTrainingCamp : SpecialBuilding
{
    public override int Cost => 100;

    public override int TimeToBuild => 100;
    public MilitaryTrainingCamp(int buildingTime, bool isFinished,  Queue<ArmyRegiment> trainingList) : base(
        buildingTime, isFinished)
    {
        TrainingList = trainingList;
    }

    public Queue<ArmyRegiment> TrainingList { get; set; }
}