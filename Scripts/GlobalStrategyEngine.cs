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
	private Timer _timer;
	public CallMulticaster AllHandlersControls;

	public Camera Camera;
	public GUI GUIHandler;

	public override void _Ready()
	{
		var allHandlers = new List<GameHandler>();

		allHandlers.Add(GetNode<SelectorBoxHandler>("./SelectionHandler"));
		allHandlers.Add(GetNode<ArmyHandler>("./ArmyHandler"));
		allHandlers.Add(GetNode<MapHandler>("./MapHandler"));


		foreach (var handler in allHandlers)
		{
			handler.ToGUIEvent += InvokeToGUIEvent;
			handler.ToEngineEvent += ReceiveToEngineEvent;
		}

		AllHandlersControls = new CallMulticaster(allHandlers);

		var map = GetNode<Sprite2D>("MapHandler/Map").Texture.GetImage();

		Scenario scenario = new EuropeScenario(map);
		EngineState.MapInfo = new MapData(scenario);
		GlobalResources.GoodSpriteFrames = GD.Load<SpriteFrames>("res://Prefabs/SpriteFrames/GoodSpriteFrames.tres");
		GlobalResources.BuildingSpriteFrames = GD.Load<SpriteFrames>("res://Prefabs/SpriteFrames/Buildings.tres");


		Camera = GetNode<Camera>("./Camera");
		Camera.ZoomChanged += ViewMode;
		Camera.Reset();

		GUIHandler = GetNode<GUI>("CanvasLayer/GUI");
		GUIHandler.GUIGlobalEvent += GUIEventHandler;
		GUIHandler.Init();

		AllHandlersControls.Init();
		InvokeToGUIEvent(new ToGUIUpdateCountryInfo());

		_timer = GetNode<Timer>("./DayTimer");
		


		InvokeToGUIEvent(new ToGUISetCamera(Camera, GetViewport()));
		InvokeToGUIEvent(new ToGUISetPause());
		_timer.Stop();
	}


	public void ViewMode()
	{
		AllHandlersControls.ViewModUpdate(Camera.Zoom.X);
	}

	public void TimeTick()
	{
		AllHandlersControls.TimeTick();
		InvokeToGUIEvent(new ToGUISetDateEvent(EngineState.MapInfo.Scenario.Date));
		_timer.Start();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		//Camera._Input(@event);
		var tileId = _findTile();
		AllHandlersControls.InputHandle(@event, tileId);
		GUIHandler.InputHandle(@event);
		if (@event is InputEventMouseMotion e)
			if (tileId > -1)
				InvokeToGUIEvent(
					new ToGUIShowInfoBoxEvent(
						InfoBoxFactory.ProvinceDataInfoBox(EngineState.MapInfo.Scenario.Map[tileId])));
	}

	public void GUIEventHandler(GUIEvent @event)
	{
		switch (@event)
		{
			case GUIChangeMapType e:
				EngineState.MapInfo.CurrentMapMode = e.NewMapType;
				ViewMode();
				return;
			case GUISetTimeScale e:
				_timer.WaitTime = Settings.TimeScale[e.TimeScaleId];
				return;
			case GUIPauseStateEvent e:
				if (e.IsPaused)
					_timer.Stop();
				else
					_timer.Start();
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
				InvokeToGUIEvent(new ToGUIShowInfoBoxEvent(e.RichTextLabelBuilder));
				return;
			case GUIHideInfoBoxEvent e:
				InvokeToGUIEvent(new ToGUIHideInfoBox());
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
				ViewMode();
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
		var iMousePos = new Vector2I((int)mousePos.X, (int)mousePos.Y);
		if (!EngineState.MapInfo.Scenario.MapTexture.GetUsedRect().HasPoint(iMousePos)) return -3;

		var tileId = GameMath.GetProvinceId(EngineState.MapInfo.Scenario.MapTexture.GetPixelv(iMousePos));

		if (tileId < 0 || tileId >= EngineState.MapInfo.Scenario.Map.Length)
			return -3;

		return tileId;
	}
}
