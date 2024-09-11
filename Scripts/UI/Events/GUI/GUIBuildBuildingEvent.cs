using EuropeDominationDemo.Scripts.Scenarios.Buildings;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public struct GUIBuildBuildingEvent : GUIEvent
{
    public Building NewBuilding;

    public GUIBuildBuildingEvent(Building building)
    {
        NewBuilding = building;
    }
}