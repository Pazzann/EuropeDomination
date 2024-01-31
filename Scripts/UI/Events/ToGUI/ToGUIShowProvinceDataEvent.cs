using EuropeDominationDemo.Scripts.Scenarios;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public class ToGuiShowProvinceDataEvent : ToGUIEvent
{
    public ProvinceData ShowProvinceData;
    public ToGuiShowProvinceDataEvent(ProvinceData showProvinceData)
    {
        ShowProvinceData = showProvinceData;
    }
}