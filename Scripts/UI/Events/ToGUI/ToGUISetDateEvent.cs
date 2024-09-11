using System;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public struct ToGUISetDateEvent : ToGUIEvent
{
    public DateTime DateTime;

    public ToGUISetDateEvent(DateTime newDateTime)
    {
        DateTime = newDateTime;
    }
}