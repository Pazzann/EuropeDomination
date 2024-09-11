using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public delegate void GUIHandlerEventDelegate(GUIEvent @event);

public abstract partial class GUIHandler : Control
{
    public abstract void Init();
    public event GUIEventDelegate GUIEvent;

    public void InvokeGUIEvent(GUIEvent @event)
    {
        GUIEvent.Invoke(@event);
    }

    public abstract void InputHandle(InputEvent @event);
    public abstract void ToGUIHandleEvent(ToGUIEvent @event);
}