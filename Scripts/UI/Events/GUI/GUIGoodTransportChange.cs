using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Goods;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public delegate void RouteAdressProvider(TransportationRoute route);
public delegate void NewTransportationRouteReciever(TransportationRoute transportationRoute);
public class GUIGoodTransportChange: GUIEvent
{
    public Good GoodToTransport;
    public double Amount;
    public RouteAdressProvider RouteAdress;
    public NewTransportationRouteReciever NewTransportationRouteReciever;

    public GUIGoodTransportChange(Good goodToTransport, double amount, RouteAdressProvider routeAdress, NewTransportationRouteReciever newTransportationRouteReciever)
    {
        GoodToTransport = goodToTransport;
        Amount = amount;
        RouteAdress = routeAdress;
        NewTransportationRouteReciever = newTransportationRouteReciever;
    }
}