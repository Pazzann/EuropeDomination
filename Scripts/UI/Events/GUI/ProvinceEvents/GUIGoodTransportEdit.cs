using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Goods;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI.ProvinceEvents;

public struct GUIGoodTransportEdit : GUIEvent
{
    public TransportationRoute TransportationRouteToEdit { get; }
    public double Amount { get; }
    public Good Good { get; }
    
    
    public GUIGoodTransportEdit(TransportationRoute transportationRouteToEdit, double amount, Good good)
    {
        TransportationRouteToEdit = transportationRouteToEdit;
        Amount = amount;
        Good = good;
    }
}