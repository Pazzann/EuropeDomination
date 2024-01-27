using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Handlers.TimeHandlers;
using EuropeDominationDemo.Scripts.Scenarios;
using Godot;

namespace EuropeDominationDemo.Scripts.Handlers;

public abstract class GameHandler : Node2D
{
	protected MapData _mapData;
	public TimeHandler TimeHandler;
	
	public abstract void Init(MapData mapData);
	
	public abstract void InputHandle(InputEvent @event, int tileId);
	public abstract void ViewModUpdate(float zoom);
	public abstract void GUIInteractionHandler(GUIEvent @event);
}