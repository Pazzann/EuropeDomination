using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Handlers;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;
using Godot;

namespace EuropeDominationDemo.Scripts;

public partial class GlobalStrategyEngine : Node
{
	public CallStack AllHandlersControls;
	public Scenario Scenario;
	public CameraBehaviour Camera;


	private Timer _timer;
	
	public override void _Ready()
	{
		List<GameHandler> allHandlers = new List<GameHandler>();
		
		allHandlers.Add(GetNode<GameHandler>("./MapHandler"));
		allHandlers.Add(GetNode<GameHandler>("./ArmyHandler"));
		
		AllHandlersControls = new CallStack(allHandlers);
		
		Image map = GD.Load("res://Sprites/map.png") as Image;
		Scenario = new DemoScenario(map);
		
		
		AllHandlersControls.Init(Scenario);
		
		_timer = GetNode<Timer>("./DayTimer");
		_timer.Start();
	}

	
	private void TimeTick()
	{
		AllHandlersControls.TimeTick();
	}

	public override void _Input(InputEvent @event)
	{
		
		AllHandlersControls.InputHandle(@event);
	}
}

