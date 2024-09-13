using EuropeDominationDemo.Scripts.Utils;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public struct ToGUIShowInfoBoxEvent : ToGUIEvent
{
    public RichTextLabelBuilder RichTextLabelBuilder;

    public ToGUIShowInfoBoxEvent(RichTextLabelBuilder richTextLabelBuilder)
    {
        RichTextLabelBuilder = richTextLabelBuilder;
    }
}