using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Handlers;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;
using Godot;

namespace EuropeDominationDemo.Scripts;

public partial class GlobalStrategyEngine : Node2D
{
	public CallStack AllHandlersControls;
	public MapData MapInfo;
	public CameraBehaviour Camera;


	private Timer _timer;
	
	public override void _Ready()
	{
		List<GameHandler> allHandlers = new List<GameHandler>();
		
		allHandlers.Add(GetNode<MapHandler>("./MapHandler"));
		allHandlers.Add(GetNode<ArmyHandler>("./ArmyHandler"));
		allHandlers.Add(GetNode<SelectorBoxHandler>("./SelectionHandler"));
		
		AllHandlersControls = new CallStack(allHandlers);
		
		Image map = GD.Load("res://Sprites/map.png") as Image;
		Scenario scenario = new DemoScenario(map);
		MapInfo = new MapData(scenario);


		Camera = GetNode<CameraBehaviour>("./Camera");
		Camera.ChangeZoom += ViewModeChange;
		
		AllHandlersControls.Init(MapInfo);
		
		_timer = GetNode<Timer>("./DayTimer");
		_timer.Start();
	}


	public void ViewModeChange()
	{
		AllHandlersControls.ViewModUpdate(Camera.Zoom.X);
	}
	
	public void TimeTick()
	{
		AllHandlersControls.TimeTick();
		_timer.Start();
	}

	public override void _Input(InputEvent @event)
	{
		var tileId = _findTile();		
		AllHandlersControls.InputHandle(@event, tileId);
	}
	
	private int _findTile()
	{
		var mousePos = GetLocalMousePosition();
		var iMousePos = new Vector2I((int)(mousePos.X), (int)(mousePos.Y));
		if (!MapInfo.Scenario.MapTexture.GetUsedRect().HasPoint(iMousePos)) return -3;
		
		var tileId = GameMath.GetProvinceID(MapInfo.Scenario.MapTexture.GetPixelv(iMousePos));

		if (tileId < 0 || tileId >= MapInfo.Scenario.Map.Length)
			return -3;

		return tileId;
	}
}

