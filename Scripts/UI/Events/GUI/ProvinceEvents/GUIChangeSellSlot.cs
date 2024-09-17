using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI.ProvinceEvents;

public struct GUIChangeSellSlot : GUIEvent
{
    public int ProvinceId { get; }
    public int TabId { get; }
    public int SlotId { get; }
    public KeyValuePair<int, double> SlotData { get; }

    public GUIChangeSellSlot(int provinceId, int tabId, int slotId, KeyValuePair<int, double> slotData)
    {
        ProvinceId = provinceId;
        TabId = tabId;
        SlotId = slotId;
        SlotData = slotData;
    }
}