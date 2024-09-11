using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public struct ToGUIShowUncolonizedProvinceData : ToGUIEvent
{
    public UncolonizedProvinceData UncolonizedProvinceData;

    public ToGUIShowUncolonizedProvinceData(UncolonizedProvinceData uncolonizedProvinceData)
    {
        UncolonizedProvinceData = uncolonizedProvinceData;
    }
}