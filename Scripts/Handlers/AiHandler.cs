﻿using System;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using Godot;

namespace EuropeDominationDemo.Scripts.Handlers;

public partial class AiHandler : GameHandler
{
    public override void Init()
    {
        
    }

    public override bool InputHandle(InputEvent @event, int tileId)
    {
        return false;
    }

    public override void ViewModUpdate(float zoom)
    {
        
    }

    public override void GUIInteractionHandler(GUIEvent @event)
    {
        
    }

    public override void DayTick()
    {
    }

    public override void MonthTick()
    {
        foreach (var aiCountry in EngineState.MapInfo.Scenario.AiList)
        {
            var country = EngineState.MapInfo.Scenario.Countries[aiCountry];
            if (EngineState.MapInfo.MapProvinces(ProvinceTypes.UncolonizedProvinces).Count(b =>
                    b is UncolonizedProvinceData data && data.CurrentlyColonizedByCountry != -1 &&
                    data.CurrentlyColonizedByCountry == country.Id) < 2 && country.Money > EngineState.MapInfo.Scenario.Settings.InitialMoneyCostColony && country.Manpower > EngineState.MapInfo.Scenario.Settings.InitialManpowerCostColony)
            {
                var colonizableProvinces = country.GetAvailibaleProvincesToColonize();
                if (colonizableProvinces.Length != 0)
                {
                    var id = new Random().Next(colonizableProvinces.Length);
                    colonizableProvinces[id].CurrentlyColonizedByCountry = country.Id;
                    country.Money -= EngineState.MapInfo.Scenario.Settings.InitialMoneyCostColony;
                    country.Manpower -= EngineState.MapInfo.Scenario.Settings.InitialManpowerCostColony;
                }
                
            }
            
        }   
    }

    public override void YearTick()
    {
        
    }
}