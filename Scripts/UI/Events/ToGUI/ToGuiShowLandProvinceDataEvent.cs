﻿using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public class ToGuiShowLandProvinceDataEvent : ToGUIEvent
{
    public LandColonizedProvinceData ShowColonizedProvinceData;
    public ToGuiShowLandProvinceDataEvent(LandColonizedProvinceData showColonizedProvinceData)
    {
        ShowColonizedProvinceData = showColonizedProvinceData;
    }
}