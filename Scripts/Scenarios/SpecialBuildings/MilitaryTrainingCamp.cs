using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class MilitaryTrainingCamp : SpecialBuilding
{
    public MilitaryTrainingCamp(int buildingTime, bool isFinished, int cost, Queue<ArmyRegiment> trainingList) : base(
        100, buildingTime, isFinished, cost)
    {
        TrainingList = trainingList;
    }

    public Queue<ArmyRegiment> TrainingList { get; set; }
}