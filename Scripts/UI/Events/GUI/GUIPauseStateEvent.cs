namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public struct GUIPauseStateEvent : GUIEvent
{
    public bool IsPaused;

    public GUIPauseStateEvent(bool isPaused)
    {
        IsPaused = isPaused;
    }
}