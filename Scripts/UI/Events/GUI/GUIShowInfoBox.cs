using EuropeDominationDemo.Scripts.UI.GUIHandlers;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public struct GUIShowInfoBox: GUIEvent
{
    public InfoBoxBuilder InfoBoxBuilder;

    public GUIShowInfoBox(InfoBoxBuilder infoBoxBuilder)
    {
        InfoBoxBuilder = infoBoxBuilder;
    }
}