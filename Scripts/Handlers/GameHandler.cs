using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Handlers.TimeHandlers;
using EuropeDominationDemo.Scripts.Scenarios;
using Godot;

namespace EuropeDominationDemo.Scripts.Handlers;

public abstract class GameHandler : Node
{
	protected Scenario _scenario;
	public TimeHandler TimeHandler;
	
	public abstract void Init(Scenario scenario);
	
	public abstract void InputHandle(InputEvent @event);
	public abstract void ViewModUpdate(MapTypes mapTypes, float zoom);
	public abstract void GUIInteractionHandler(GUIEvent @event);
}