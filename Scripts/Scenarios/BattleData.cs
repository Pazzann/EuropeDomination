using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class BattleData
{
    public Regiment[,] Battlefield { get; }
    public UnitData Attacker { get; }
    public UnitData Defender { get; }

    public BattleData(ArmyUnitData attacker, ArmyUnitData defender)
    {
        Battlefield = new Regiment[20, 20];
        Attacker = attacker;
        Defender = defender;

        var infantry = new Stack<ArmyInfantryRegiment>(attacker.Regiments.OfType<ArmyInfantryRegiment>());
        var cavalry = new Stack<ArmyCavalryRegiment>(attacker.Regiments.OfType<ArmyCavalryRegiment>());
        var artillery = new Stack<ArmyArtilleryRegiment>(attacker.Regiments.OfType<ArmyArtilleryRegiment>());

        int[,] positions = {
            { 18, 0 }, { 18, 1 }, { 18, 18 }, { 18, 19 },
            { 19, 0 }, { 19, 1 }, { 19, 18 }, { 19, 19 }
        };

        foreach (var position in positions)
        {
            if (cavalry.Count > 0)
            {
                Battlefield[position[0], position[1]] = cavalry.Pop();
            }
        }

        var firstRowColumnLeft = 10;
        var firstRowColumnRight = 11;

        while (infantry.Count != 0)
        {
            var regiment = infantry.Pop();

            if (10 - firstRowColumnLeft >= firstRowColumnRight - 11)
            {
                Battlefield[18, firstRowColumnRight] = regiment;

                if (firstRowColumnRight == 19)
                    break;

                firstRowColumnRight++;
            }
            else
            {
                Battlefield[18, firstRowColumnLeft] = regiment;

                if (firstRowColumnLeft == 0)
                    break;

                firstRowColumnLeft--;
            }
        }

        foreach (var regiment in attacker.Regiments)
        {
            if (!used.Contains(regiment) && regiment is ArmyInfantryRegiment)
            {
                if (10 - firstRowColumnLeft >= firstRowColumnRight - 11)
                {
                    Battlefield[18, firstRowColumnRight] = regiment;

                    if (firstRowColumnRight == 19)
                        break;

                    firstRowColumnRight++;
                }
                else
                {
                    Battlefield[18, firstRowColumnLeft] = regiment;

                    if (firstRowColumnLeft == 0)
                        break;

                    firstRowColumnLeft--;
                }

                used.Add(regiment);
            }
        }

        var secondRowColumnLeft = 10;
        var secondRowColumnRight = 11;

        foreach (var regiment in attacker.Regiments)
        {
            if (!used.Contains(regiment) && regiment is ArmyArtilleryRegiment)
            {
                if (10 - secondRowColumnLeft >= secondRowColumnRight - 11)
                {
                    Battlefield[19, secondRowColumnRight] = regiment;

                    if (secondRowColumnRight == 19)
                        break;

                    secondRowColumnRight++;
                }
                else
                {
                    Battlefield[19, secondRowColumnLeft] = regiment;

                    if (secondRowColumnLeft == 0)
                        break;

                    secondRowColumnLeft--;
                }

                used.Add(regiment);
            }
        }


        
    }

    public void DayTick()
    {
        
    }
}