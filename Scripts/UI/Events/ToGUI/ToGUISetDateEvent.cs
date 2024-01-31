using System;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public class ToGUISetDateEvent : ToGUIEvent
{
    public DateTime DateTime;

    public ToGUISetDateEvent(DateTime newDateTime)
    {
        DateTime = newDateTime;
    }
}