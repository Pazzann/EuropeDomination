using Godot;
using System;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Handlers;
using EuropeDominationDemo.Scripts.Handlers.TimeHandlers;
using EuropeDominationDemo.Scripts.Scenarios;

public partial class ArmyHandler : GameHandler
{
	public override void Init(Scenario scenario)
	{

		TimeHandler = new ArmyTimeHandler();
		_scenario = scenario;
		
		throw new NotImplementedException();
	}

	public override void InputHandle(InputEvent @event)
	{
		throw new NotImplementedException();
	}

	public override void ViewModUpdate(MapTypes mapTypes, float zoom)
	{
		throw new NotImplementedException();
	}

	public override void GUIInteractionHandler(GUIEvent @event)
	{
		throw new NotImplementedException();
	}
}
