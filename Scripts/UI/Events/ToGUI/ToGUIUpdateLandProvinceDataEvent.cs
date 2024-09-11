using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public struct ToGUIUpdateLandProvinceDataEvent : ToGUIEvent
{
    public LandColonizedProvinceData UpdateColonizedProvinceData;

    public ToGUIUpdateLandProvinceDataEvent(LandColonizedProvinceData updateColonizedProvinceData)
    {
        UpdateColonizedProvinceData = updateColonizedProvinceData;
    }
}