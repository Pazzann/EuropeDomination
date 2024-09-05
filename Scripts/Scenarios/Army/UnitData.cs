using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.Scenarios.Army;

public class UnitData
{
    public string Name { get; set; }
    public int Owner { get; set; }

    public int CurrentProvince { get; set; }
    
    public Modifiers Modifiers { get; set; }
    
    public List<KeyValuePair<int,int>> MovementQueue { get; set; }
    public int MovementProgress;

    public UnitData(string name, int owner, int currentProvince, Modifiers modifiers,List<KeyValuePair<int,int>> movementQueue, int movementProgress)
    {
        Name = name;
        CurrentProvince = currentProvince;
        Modifiers = modifiers;
        Owner = owner;
        MovementQueue = movementQueue;
        MovementProgress = movementProgress;
    }
    
    public bool AddDay()
    {
        MovementProgress++;
        return MovementProgress >= MovementQueue[^1].Value;
    }

    public int TotalDistance
    {
        get
        {
            var d = 0;
            foreach (var pair in MovementQueue)
            {
                d += pair.Value;
            }

            return d;
        }
    }
}