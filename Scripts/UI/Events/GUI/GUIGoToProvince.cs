
using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public class GUIGoToProvince : GUIEvent
{
    public int Id;

    public GUIGoToProvince(int id)
    {
        Id = id;
    }
}