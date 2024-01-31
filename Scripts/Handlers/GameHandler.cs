using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;

namespace EuropeDominationDemo.Scripts.Handlers;

public delegate void ToGUIEventSender(ToGUIEvent @event);

public abstract partial class GameHandler : Node2D
{
	protected MapData _mapData;

	public event ToGUIEventSender ToGUIEvent = null;

	public void InvokeToGUIEvent(ToGUIEvent @event)
	{
		ToGUIEvent.Invoke(@event);
	}
	
	public abstract void Init(MapData mapData);
	
	public abstract bool InputHandle(InputEvent @event, int tileId);
	public abstract void ViewModUpdate(float zoom);
	public abstract void GUIInteractionHandler(GUIEvent @event);
	
	
	public abstract void DayTick();
	public abstract void MonthTick();
	public abstract void YearTick();
}