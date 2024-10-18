using System;
using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

[Serializable]
public class BattleData
{
    //add dice

    public BattleData(ArmyUnitData attacker, ArmyUnitData defender)
    {
        Battlefield = new Regiment[20, 20];
        Attacker = attacker;
        Defender = defender;

        _battleDataPlacement(18, 19, attacker);
        _battleDataPlacement(1, 0, defender);
    }

    public Regiment[,] Battlefield { get; }
    public UnitData Attacker { get; }
    public UnitData Defender { get; }

    private void _battleDataPlacement(int frontRow, int backRow, ArmyUnitData army)
    {
        var infantry = new Stack<ArmyInfantryRegiment>(army.Regiments.OfType<ArmyInfantryRegiment>());
        var cavalry = new Stack<ArmyCavalryRegiment>(army.Regiments.OfType<ArmyCavalryRegiment>());
        var artillery = new Stack<ArmyArtilleryRegiment>(army.Regiments.OfType<ArmyArtilleryRegiment>());

        //placing corner cavalry
        Vector2I[] cavalryPrimaryPositions =
        {
            new(frontRow, 0), new(frontRow, 19), new(frontRow, 1), new(frontRow, 18),
            new(backRow, 0), new(backRow, 19), new(backRow, 1), new(backRow, 18)
        };
        for (var positionIndex = 0;
             positionIndex < cavalryPrimaryPositions.Length && cavalry.Count > 0;
             positionIndex++)
        {
            var position = cavalryPrimaryPositions[positionIndex];
            Battlefield[position.X, position.Y] = cavalry.Pop();
        }

        //filling front row
        for (var rightPosition = 10;
             rightPosition < 20 && Battlefield[frontRow, rightPosition] == null &&
             (infantry.Count > 0 || cavalry.Count > 0);
             rightPosition++)
        {
            if (infantry.Count > 0) Battlefield[frontRow, rightPosition] = infantry.Pop();
            else Battlefield[frontRow, rightPosition] = cavalry.Pop();
            if (infantry.Count > 0 || cavalry.Count > 0)
            {
                var leftPosition = 19 - rightPosition;
                if (Battlefield[frontRow, leftPosition] == null)
                {
                    if (infantry.Count > 0) Battlefield[frontRow, leftPosition] = infantry.Pop();
                    else Battlefield[frontRow, leftPosition] = cavalry.Pop();
                }
            }
        }

        //filling back row
        for (var rightPosition = 10;
             rightPosition < 20 && Battlefield[backRow, rightPosition] == null &&
             (artillery.Count > 0 || infantry.Count > 0 || cavalry.Count > 0);
             rightPosition++)
        {
            if (artillery.Count > 0) Battlefield[backRow, rightPosition] = artillery.Pop();
            else if (infantry.Count > 0) Battlefield[backRow, rightPosition] = infantry.Pop();
            else Battlefield[backRow, rightPosition] = cavalry.Pop();
            if (artillery.Count > 0 || infantry.Count > 0 || cavalry.Count > 0)
            {
                var leftPosition = 19 - rightPosition;
                if (Battlefield[backRow, leftPosition] == null)
                {
                    if (artillery.Count > 0) Battlefield[backRow, leftPosition] = artillery.Pop();
                    else if (infantry.Count > 0) Battlefield[backRow, leftPosition] = infantry.Pop();
                    else Battlefield[backRow, leftPosition] = cavalry.Pop();
                }
            }
        }
    }

    public void DayTick()
    {
    }
}