﻿using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

public class ArmyInfantryRegiment : ArmyRegiment
{
    public InfantryWeapon Weapon;
    public Helmet Helmet;
    public Armor Armor;
    public Boots Boots;
    public AdditionalSlotGood AdditionalSlot;
    public ArmyInfantryRegiment(string name, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, int maxManpower) : base(name, templateId, timeFromStartOfTheTraining, trainingTime, isFinished, manpower, maxManpower)
    {
        
    }

   
}