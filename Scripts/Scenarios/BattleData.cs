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

        _placeUnit(18, 19, attacker);
        _placeUnit(1, 0, defender);
        _setTargets();
    }

    public Regiment[,] Battlefield { get; }
    public ArmyUnitData Attacker { get; }
    public ArmyUnitData Defender { get; }
    
    public List<ArmyRegiment> placedAttackers { get; }
    public List<ArmyRegiment> placedDefenders { get; }

    private void _placeUnit(int frontRow, int backRow, ArmyUnitData army)
    {
        var infantry = new Stack<ArmyInfantryRegiment>(army.Regiments.OfType<ArmyInfantryRegiment>());
        var cavalry = new Stack<ArmyCavalryRegiment>(army.Regiments.OfType<ArmyCavalryRegiment>());
        var artillery = new Stack<ArmyArtilleryRegiment>(army.Regiments.OfType<ArmyArtilleryRegiment>());

        var placedRegiments = new List<ArmyRegiment>();

        //placing corner cavalry
        Vector2I[] cavalryPrimaryPositions =
        [
            new(frontRow, 0), new(frontRow, 19), new(frontRow, 1), new(frontRow, 18),
            new(backRow, 0), new(backRow, 19), new(backRow, 1), new(backRow, 18)
        ];
        for (var positionIndex = 0;
             positionIndex < cavalryPrimaryPositions.Length && cavalry.Count > 0;
             positionIndex++)
        {
            _placeRegiment(cavalry.Pop(), cavalryPrimaryPositions[positionIndex]);
        }

        //filling front row
        for (var rightPosition = 10;
             rightPosition < 20 && Battlefield[frontRow, rightPosition] == null &&
             (infantry.Count > 0 || cavalry.Count > 0);
             rightPosition++)
        {
            if (infantry.Count > 0)
                _placeRegiment(infantry.Pop(), new(frontRow, rightPosition));
            else
                _placeRegiment(cavalry.Pop(), new(frontRow, rightPosition));
            if (infantry.Count > 0 || cavalry.Count > 0)
            {
                var leftPosition = 19 - rightPosition;
                if (Battlefield[frontRow, leftPosition] == null)
                {
                    if (infantry.Count > 0)
                        _placeRegiment(infantry.Pop(), new(frontRow, leftPosition));
                    else
                        _placeRegiment(cavalry.Pop(), new(frontRow, leftPosition));
                }
            }
        }

        //filling back row
        for (var rightPosition = 10;
             rightPosition < 20 && Battlefield[backRow, rightPosition] == null &&
             (artillery.Count > 0 || infantry.Count > 0 || cavalry.Count > 0);
             rightPosition++)
        {
            if (artillery.Count > 0)
                _placeRegiment(artillery.Pop(), new(backRow, rightPosition));
            else if (infantry.Count > 0)
                _placeRegiment(infantry.Pop(), new(backRow, rightPosition));
            else
                _placeRegiment(cavalry.Pop(), new(backRow, rightPosition));
            if (artillery.Count > 0 || infantry.Count > 0 || cavalry.Count > 0)
            {
                var leftPosition = 19 - rightPosition;
                if (Battlefield[backRow, leftPosition] == null)
                {
                    if (artillery.Count > 0)
                        _placeRegiment(artillery.Pop(), new(backRow, leftPosition));
                    else if (infantry.Count > 0)
                        _placeRegiment(infantry.Pop(), new(backRow, leftPosition));
                    else
                        _placeRegiment(cavalry.Pop(), new(backRow, leftPosition));
                }
            }
        }
    }

    private void _placeRegiment(ArmyRegiment regiment, Vector2I position)
    {
        regiment.Position = position;
        Battlefield[position.X, position.Y] = regiment;
    }

    private void _setTargets()
    {
        foreach (var defenderRegiment in Defender.Regiments)
            if (defenderRegiment.Position != null)
            {
                fr
            }
    }

    public void DayTick()
    {
        for (var x = 0; x < 20; x++)
        for (var y = 0; y < 20; y++)
        {
            
        }
        /*foreach (unit in Units)
        {
            if () Move(unit);
            else Attack(unit,);
        }*/
    }
}