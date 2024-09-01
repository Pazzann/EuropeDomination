using Godot;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public class ToGUISetCamera : ToGUIEvent
{
    public CameraBehaviour Camera;
    public Viewport Viewport;

    public ToGUISetCamera(CameraBehaviour camera, Viewport viewport)
    {
        Camera = camera;
        Viewport = viewport;
    }
}