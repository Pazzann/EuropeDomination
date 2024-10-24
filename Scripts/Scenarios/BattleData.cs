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

        PlacedAttackerRegiments = _placeUnit(18, 19, attacker);
        PlacedDefenderRegiments = _placeUnit(1, 0, defender);
        _setTargets();
    }

    public Regiment[,] Battlefield { get; }
    public ArmyUnitData Attacker { get; }
    public ArmyUnitData Defender { get; }
    
    public List<ArmyRegiment> PlacedAttackerRegiments { get; }
    public List<ArmyRegiment> PlacedDefenderRegiments { get; }

    //places the maximum possible amount of regiments of army using _placeRegiment
    private List<ArmyRegiment> _placeUnit(int frontRow, int backRow, ArmyUnitData army)
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
             ++positionIndex)
        {
            _placeRegiment(cavalry.Pop(), cavalryPrimaryPositions[positionIndex], placedRegiments);
        }

        //filling front row
        for (var rightPosition = 10;
             rightPosition < 20 && Battlefield[frontRow, rightPosition] == null &&
             (infantry.Count > 0 || cavalry.Count > 0);
             ++rightPosition)
        {
            if (infantry.Count > 0)
                _placeRegiment(infantry.Pop(), new(frontRow, rightPosition), placedRegiments);
            else
                _placeRegiment(cavalry.Pop(), new(frontRow, rightPosition), placedRegiments);
            if (infantry.Count > 0 || cavalry.Count > 0)
            {
                var leftPosition = 19 - rightPosition;
                if (Battlefield[frontRow, leftPosition] == null)
                {
                    if (infantry.Count > 0)
                        _placeRegiment(infantry.Pop(), new(frontRow, leftPosition), placedRegiments);
                    else
                        _placeRegiment(cavalry.Pop(), new(frontRow, leftPosition), placedRegiments);
                }
            }
        }

        //filling back row
        for (var rightPosition = 10;
             rightPosition < 20 && Battlefield[backRow, rightPosition] == null &&
             (artillery.Count > 0 || infantry.Count > 0 || cavalry.Count > 0);
             ++rightPosition)
        {
            if (artillery.Count > 0)
                _placeRegiment(artillery.Pop(), new(backRow, rightPosition), placedRegiments);
            else if (infantry.Count > 0)
                _placeRegiment(infantry.Pop(), new(backRow, rightPosition), placedRegiments);
            else
                _placeRegiment(cavalry.Pop(), new(backRow, rightPosition), placedRegiments);
            if (artillery.Count > 0 || infantry.Count > 0 || cavalry.Count > 0)
            {
                var leftPosition = 19 - rightPosition;
                if (Battlefield[backRow, leftPosition] == null)
                {
                    if (artillery.Count > 0)
                        _placeRegiment(artillery.Pop(), new(backRow, leftPosition), placedRegiments);
                    else if (infantry.Count > 0)
                        _placeRegiment(infantry.Pop(), new(backRow, leftPosition), placedRegiments);
                    else
                        _placeRegiment(cavalry.Pop(), new(backRow, leftPosition), placedRegiments);
                }
            }
        }

        return placedRegiments;
    }

    //sets regiment Position, adds it to Battlefield and placedRegiments
    private void _placeRegiment(ArmyRegiment regiment, Vector2I position, List<ArmyRegiment> placedRegiments)
    {
        regiment.Position = position;
        Battlefield[position.X, position.Y] = regiment;
        placedRegiments.Add(regiment);
    }
    
    //sets the Targets of PlacedAttackerRegiments & PlacedDefenderRegiments
    private void _setTargets()
    {
        foreach (var defenderRegiment in PlacedDefenderRegiments)
            foreach (var attackerRegiment in PlacedAttackerRegiments)
                if (defenderRegiment.Target == null ||
                    defenderRegiment.DistanceTo(attackerRegiment.Position) <
                    defenderRegiment.DistanceTo(defenderRegiment.Target?.Position))
                    defenderRegiment.Target = attackerRegiment;
        
        foreach (var attackerRegiment in PlacedAttackerRegiments)
            foreach (var defenderRegiment in PlacedDefenderRegiments)
                if (attackerRegiment.Target == null ||
                    attackerRegiment.DistanceTo(defenderRegiment.Position) <
                    attackerRegiment.DistanceTo(attackerRegiment.Target?.Position))
                    attackerRegiment.Target = defenderRegiment;
    }
    
    //reflects the move of the regiment on oldPosition (made by the corresponding Regiment method) on the Battlefield 
    private void _moveRegimentOnBattlefield(Vector2I oldPosition)
    {
        var regiment = Battlefield[oldPosition.X, oldPosition.Y];
        if (regiment.Position is { } newPosition)
        {
            Battlefield[newPosition.X, newPosition.Y] = regiment;
            Battlefield[oldPosition.X, oldPosition.Y] = null;
        }
    }
    

    public void DayTick()
    {
        var placedRegiments = new List<ArmyRegiment>(PlacedAttackerRegiments);
        placedRegiments.AddRange(PlacedDefenderRegiments);
        foreach (var regiment in placedRegiments)
        {
            if (regiment.HasInRange(regiment.Target?.Position)) regiment.AttackRegiment(regiment.Target);
            else
                if (regiment.Position is { } oldPosition)      //formal check, regiment.Position must be set
                {
                    regiment.MoveToTarget(Battlefield);
                    _moveRegimentOnBattlefield(oldPosition);
                }
        }
    }
}