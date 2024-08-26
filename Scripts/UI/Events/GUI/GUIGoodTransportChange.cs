using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public class GUIGoodTransportChange : GUIEvent
{
    public Good GoodToTransport;

    public GUIGoodTransportChange(Good goodToTransport)
    {
        GoodToTransport = goodToTransport;
    }
}