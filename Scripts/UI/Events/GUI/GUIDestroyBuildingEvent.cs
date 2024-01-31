namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public class GUIDestroyBuildingEvent : GUIEvent
{
    public int DestroyedId;

    public GUIDestroyBuildingEvent(int destroyedId)
    {
        DestroyedId = destroyedId;
    }
}