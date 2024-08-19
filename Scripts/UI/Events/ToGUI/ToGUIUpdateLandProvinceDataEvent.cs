using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public class ToGUIUpdateLandProvinceDataEvent : ToGUIEvent
{
    public LandProvinceData UpdateProvinceData;

    public ToGUIUpdateLandProvinceDataEvent(LandProvinceData updateProvinceData)
    {
        UpdateProvinceData = updateProvinceData;
    }
}