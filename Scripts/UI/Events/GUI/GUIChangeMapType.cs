
using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public class GUIChangeMapType : GUIEvent
{
    public MapTypes NewMapType;

    public GUIChangeMapType(MapTypes newMapType)
    {
        NewMapType = newMapType;
    }
}