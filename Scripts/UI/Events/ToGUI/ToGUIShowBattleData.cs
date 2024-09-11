using EuropeDominationDemo.Scripts.Scenarios;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public struct ToGUIShowBattleData : ToGUIEvent
{
    public BattleData BattleData;

    public ToGUIShowBattleData(BattleData battleData)
    {
        BattleData = battleData;
    }
}