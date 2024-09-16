using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public struct ToGUISetCamera : ToGUIEvent
{
    public Camera Camera;
    public Viewport Viewport;

    public ToGUISetCamera(Camera camera, Viewport viewport)
    {
        Camera = camera;
        Viewport = viewport;
    }
}