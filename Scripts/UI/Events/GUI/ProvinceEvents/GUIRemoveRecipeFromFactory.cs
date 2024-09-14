namespace EuropeDominationDemo.Scripts.UI.Events.GUI.ProvinceEvents;

public struct GUIRemoveRecipeFromFactory : GUIEvent
{
    public int ProvinceId { get; }
    public int TabId { get; }

    public GUIRemoveRecipeFromFactory(int provinceId, int tabId)
    {
        ProvinceId = provinceId;
        TabId = tabId;
    }

}