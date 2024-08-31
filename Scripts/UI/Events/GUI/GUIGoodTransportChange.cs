using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public delegate void RouteAdressProvider(TransportationRoute route);
public class GUIGoodTransportChange: GUIEvent
{
    public Good GoodToTransport;
    public int Amount;
    public RouteAdressProvider RouteAdress;

    public GUIGoodTransportChange(Good goodToTransport, int amount, RouteAdressProvider routeAdress)
    {
        GoodToTransport = goodToTransport;
        Amount = amount;
        RouteAdress = routeAdress;
    }
}