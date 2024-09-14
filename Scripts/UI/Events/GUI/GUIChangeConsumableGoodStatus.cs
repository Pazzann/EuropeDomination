namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public struct GUIChangeConsumableGoodStatus : GUIEvent
{
    public int GoodId { get; set; }

    public GUIChangeConsumableGoodStatus(int goodId)
    {
        GoodId = goodId;
    }
}