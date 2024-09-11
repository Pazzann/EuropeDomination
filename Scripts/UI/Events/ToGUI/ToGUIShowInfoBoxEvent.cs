using EuropeDominationDemo.Scripts.UI.GUIHandlers;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public struct ToGUIShowInfoBoxEvent : ToGUIEvent
{
    public InfoBoxBuilder InfoBoxBuilder;

    public ToGUIShowInfoBoxEvent(InfoBoxBuilder infoBoxBuilder)
    {
        InfoBoxBuilder = infoBoxBuilder;
    }
}