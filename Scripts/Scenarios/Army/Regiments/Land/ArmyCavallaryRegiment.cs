﻿using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

public class ArmyCavallaryRegiment : ArmyRegiment
{
    public InfantryWeapon Weapon;
    public Horse Horse;
    public Helmet Helmet;
    public Armor Armor;
    public Boots Boots;
    public AdditionalSlotGood AdditionalSlot;
    public ArmyCavallaryRegiment(string name, int cost, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, int maxManpower) : base(name, cost, templateId, timeFromStartOfTheTraining, trainingTime, isFinished, manpower, maxManpower)
    {
        
    }
}