using EuropeDominationDemo.Scripts.Scenarios.Buildings;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public class GUIBuildBuildingEvent : GUIEvent
{
    public Building NewBuilding;

    public GUIBuildBuildingEvent(Building building)
    {
        NewBuilding = building;
    }
}