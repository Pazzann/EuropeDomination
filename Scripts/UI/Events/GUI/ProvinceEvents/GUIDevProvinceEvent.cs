namespace EuropeDominationDemo.Scripts.UI.Events.GUI.ProvinceEvents;

public struct GUIDevProvinceEvent : GUIEvent
{
    public int ProvinceId { get; }

    public GUIDevProvinceEvent(int provinceId)
    {
        ProvinceId = provinceId;
    }
}