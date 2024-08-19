using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public class ToGuiShowLandProvinceDataEvent : ToGUIEvent
{
    public LandProvinceData ShowProvinceData;
    public ToGuiShowLandProvinceDataEvent(LandProvinceData showProvinceData)
    {
        ShowProvinceData = showProvinceData;
    }
}