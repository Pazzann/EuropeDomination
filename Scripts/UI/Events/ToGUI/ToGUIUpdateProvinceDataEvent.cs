using EuropeDominationDemo.Scripts.Scenarios;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public class ToGUIUpdateProvinceDataEvent : ToGUIEvent
{
    public ProvinceData UpdateProvinceData;

    public ToGUIUpdateProvinceDataEvent(ProvinceData updateProvinceData)
    {
        UpdateProvinceData = updateProvinceData;
    }
}