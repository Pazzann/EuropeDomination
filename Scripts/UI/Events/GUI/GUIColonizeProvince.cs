namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public struct GUIColonizeProvince : GUIEvent
{
    public int ProvinceId;

    public GUIColonizeProvince(int provinceId)
    {
        ProvinceId = provinceId;
    }
}