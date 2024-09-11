namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public struct GUIDestroyBuildingEvent : GUIEvent
{
    public int DestroyedId;

    public GUIDestroyBuildingEvent(int destroyedId)
    {
        DestroyedId = destroyedId;
    }
}