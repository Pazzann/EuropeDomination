namespace EuropeDominationDemo.Scripts.UI.Events.GUI.ProvinceEvents;

public struct GUISpecialBuildingBuild : GUIEvent
{
    public int ProvinceId { get; }
    public int SpecialBuildingId { get; }
    
    public int TabId { get; }

    public GUISpecialBuildingBuild(int provinceId, int specialBuildingId, int tabId)
    {
        ProvinceId = provinceId;
        SpecialBuildingId = specialBuildingId;
        TabId = tabId;
    }
}