using System.Collections.Generic;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Handlers;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;
using EuropeDominationDemo.Scripts.UI;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToEngine;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;
using Godot;

namespace EuropeDominationDemo.Scripts;

public partial class GlobalStrategyEngine : Node2D
{
	public CallMulticaster AllHandlersControls;

	public CameraBehaviour Camera;
	public GUI GUIHandler;

	private Timer _timer;
	private Timer _mouseInactivityTimer;
	
	public override void _Ready()
	{
		List<GameHandler> allHandlers = new List<GameHandler>();
		
		allHandlers.Add(GetNode<SelectorBoxHandler>("./SelectionHandler"));
		allHandlers.Add(GetNode<ArmyHandler>("./ArmyHandler"));
		allHandlers.Add(GetNode<MapHandler>("./MapHandler"));
		

		foreach (var handler in allHandlers)
		{
			handler.ToGUIEvent += InvokeToGUIEvent;
			handler.ToEngineEvent += ReceiveToEngineEvent;
		}
		
		AllHandlersControls = new CallMulticaster(allHandlers);
		
		var map = Image.LoadFromFile("res://Sprites/EuropeMap.png");
		
		Scenario scenario = new EuropeScenario(map);
		EngineState.MapInfo = new MapData(scenario);
		GlobalResources.GoodSpriteFrames = GD.Load<SpriteFrames>("res://Prefabs/SpriteFrames/GoodSpriteFrames.tres");
		

		Camera = GetNode<CameraBehaviour>("./Camera");
		Camera.ChangeZoom += ViewModeChange;
		Camera.LimitToMap();

		GUIHandler = GetNode<GUI>("CanvasLayer/GUI");
		GUIHandler.GUIGlobalEvent += GUIEventHandler;
		GUIHandler.Init();
		
		AllHandlersControls.Init();
		InvokeToGUIEvent(new ToGUIUpdateCountryInfo());
		
		_timer = GetNode<Timer>("./DayTimer");
		_timer.Start();

		_mouseInactivityTimer = GetNode<Timer>("./MouseinactivityTimer");
		
		InvokeToGUIEvent(new ToGUISetCamera(Camera, GetViewport()));
	}


	public void ViewModeChange()
	{
		AllHandlersControls.ViewModUpdate(Camera.Zoom.X);
	}

	private void _onMouseInactivityTimerTimeout()
	{
		var tile = _findTile();
		if(tile > -1)
			InvokeToGUIEvent(new ToGUIShowInfoBoxEvent(InfoBoxFactory.ProvinceDataInfoBox(EngineState.MapInfo.Scenario.Map[tile])));
	}
	
	public void TimeTick()
	{
		AllHandlersControls.TimeTick();
		InvokeToGUIEvent(new ToGUISetDateEvent(EngineState.MapInfo.Scenario.Date));
		_timer.Start();
	}
	
	public override void _UnhandledInput  (InputEvent @event)
	{
		Camera.InputHandle(@event);
		var tileId = _findTile();
		AllHandlersControls.InputHandle(@event, tileId);
		GUIHandler.InputHandle(@event);
		if (@event is InputEventMouseMotion e)
		{
			_mouseInactivityTimer.Stop();
			_mouseInactivityTimer.Start();
			InvokeToGUIEvent(new ToGUIHideInfoBox());
		}
	}

	public void GUIEventHandler(GUIEvent @event)
	{
		switch (@event)
		{
			case GUIChangeMapType e:
				EngineState.MapInfo.CurrentMapMode = e.NewMapType;
				ViewModeChange();
				return;
			case GUIPauseStateEvent e:
				if (e.IsPaused)
				{
					_timer.Stop();
				}
				else
				{
					_timer.Start();
				}
				return;
			case GUISwitchCountry:
				InvokeToGUIEvent(new ToGUIUpdateCountryInfo());
				return;
			case GUIShowCountryWindowEvent:
				InvokeToGUIEvent(new ToGUIShowCountryWindowEvent());
				return;
			case GUIGoToProvince e:
				Camera.GoToProvince(e.Id);
				return;
			case GUIShowInfoBox e:
				InvokeToGUIEvent(new ToGUIShowInfoBoxEvent(e.InfoBoxBuilder));
				return;
			default:
				AllHandlersControls.GUIInteractionHandler(@event);
				return;
		}
	}

	public void InvokeToGUIEvent(ToGUIEvent @event)
	{
		GUIHandler.ToGUIEventHandler(@event);
	}

	public void ReceiveToEngineEvent(ToEngine @event)
	{
		switch (@event)
		{
			case ToEngineViewModUpdate:
				ViewModeChange();
				return;
			case ToEngineAddArmyUnitEvent e:
				AllHandlersControls.GUIInteractionHandler(new GUIAddArmyUnitEvent(e.ArmyUnitData));
				return;
			default:
				return;
		}
	}
	
	private int _findTile()
	{
		var mousePos = GetLocalMousePosition();
		var iMousePos = new Vector2I((int)(mousePos.X), (int)(mousePos.Y));
		if (!EngineState.MapInfo.Scenario.MapTexture.GetUsedRect().HasPoint(iMousePos)) return -3;
		
		var tileId = GameMath.GetProvinceId(EngineState.MapInfo.Scenario.MapTexture.GetPixelv(iMousePos));

		if (tileId < 0 || tileId >= EngineState.MapInfo.Scenario.Map.Length)
			return -3;

		return tileId;
	}
}
