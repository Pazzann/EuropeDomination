namespace EuropeDominationDemo.Scripts.UI.Events.GUI.ProvinceEvents;

public struct GUISetRecipyInFactory : GUIEvent
{
    public int ProvinceId { get; }
    public int TabId { get; }
    public int RecipyId { get; }

    public GUISetRecipyInFactory(int provinceId, int tabId, int recipyId)
    {
        ProvinceId = provinceId;
        TabId = tabId;
        RecipyId = recipyId;
    }
}