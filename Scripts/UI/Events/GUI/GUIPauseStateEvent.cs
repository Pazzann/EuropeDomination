namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public class GUIPauseStateEvent : GUIEvent
{
    public bool IsPaused;

    public GUIPauseStateEvent(bool isPaused)
    {
        IsPaused = isPaused;
    }
}