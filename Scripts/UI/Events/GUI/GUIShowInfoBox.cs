using EuropeDominationDemo.Scripts.Utils;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public struct GUIShowInfoBox : GUIEvent
{
    public RichTextLabelBuilder RichTextLabelBuilder;

    public GUIShowInfoBox(RichTextLabelBuilder richTextLabelBuilder)
    {
        RichTextLabelBuilder = richTextLabelBuilder;
    }
}