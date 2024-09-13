namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public struct GUISetTimeScale : GUIEvent
{
    public int TimeScaleId { get; set; }

    public GUISetTimeScale(int timeScaleId)
    {
        TimeScaleId = timeScaleId;
    }
}