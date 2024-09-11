using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public struct ToGUIShowUncolonizedProvinceData : ToGUIEvent
{
    public UncolonizedProvinceData UncolonizedProvinceData;
    public bool CanBeColonized;

    public ToGUIShowUncolonizedProvinceData(UncolonizedProvinceData uncolonizedProvinceData, bool canBeColonized)
    {
        UncolonizedProvinceData = uncolonizedProvinceData;
        CanBeColonized = canBeColonized;
    }
}