using Godot;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public struct GUIBeginResearch : GUIEvent
{
    public Vector3I TechnologyId;

    public GUIBeginResearch(Vector3I technologyId)
    {
        TechnologyId = technologyId;
    }
}