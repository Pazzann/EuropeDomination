using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public class ToGUIShowInfoBoxEvent : ToGUIEvent
{
    public InfoBoxBuilder InfoBoxBuilder;

    public ToGUIShowInfoBoxEvent(InfoBoxBuilder infoBoxBuilder)
    {
        InfoBoxBuilder = infoBoxBuilder;
    }
}