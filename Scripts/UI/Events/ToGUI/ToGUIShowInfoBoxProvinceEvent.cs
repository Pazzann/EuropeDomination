using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public class ToGUIShowInfoBoxProvinceEvent : ToGUIEvent
{
    public ProvinceData ProvinceData;

    public ToGUIShowInfoBoxProvinceEvent(ProvinceData provinceData)
    {
        ProvinceData = provinceData;
    }
}