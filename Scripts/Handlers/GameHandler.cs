using EuropeDominationDemo.Scripts.UI.Events.ToEngine;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;

namespace EuropeDominationDemo.Scripts.Handlers;

public delegate void ToGUIEventSender(ToGUIEvent @event);

public delegate void ToEngineEventSender(ToEngine @event);

public abstract partial class GameHandler : Node2D
{
    public event ToGUIEventSender ToGUIEvent;
    public event ToEngineEventSender ToEngineEvent;

    public void InvokeToEngineEvent(ToEngine @event)
    {
        ToEngineEvent.Invoke(@event);
    }

    public void InvokeToGUIEvent(ToGUIEvent @event)
    {
        ToGUIEvent.Invoke(@event);
    }

    public abstract void Init();

    public abstract bool InputHandle(InputEvent @event, int tileId);
    public abstract void ViewModUpdate(float zoom);
    public abstract void GUIInteractionHandler(GUIEvent @event);


    public abstract void DayTick();
    public abstract void MonthTick();
    public abstract void YearTick();
}