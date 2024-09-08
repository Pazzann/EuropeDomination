using EuropeDominationDemo.Scripts.Scenarios;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public class ToGUIShowBattleData : ToGUIEvent
{
    public BattleData BattleData;

    public ToGUIShowBattleData(BattleData battleData)
    {
        BattleData = battleData;
    }
}