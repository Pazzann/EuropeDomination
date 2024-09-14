namespace EuropeDominationDemo.Scripts.UI.Events.GUI.ProvinceEvents;

public struct GUIEnqueArmyRegiment : GUIEvent
{
    public int ProvinceId { get; }
    public int TabId { get; }
    public int TemplateId { get; }

    public GUIEnqueArmyRegiment(int provinceId, int tabId, int templateId)
    {
        ProvinceId = provinceId;
        TabId = tabId;
        TemplateId = templateId;
    }
}