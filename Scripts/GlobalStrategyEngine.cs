using System.Collections.Generic;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Handlers;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.UI;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToEngine;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts;

public partial class GlobalStrategyEngine : Node2D
{
	private Godot.Timer _timer;
	public CallMulticaster AllHandlersControls;

	public Camera Camera;
	public GUI GUIHandler;

	public override void _Ready()
	{
		var allHandlers = new List<GameHandler>
		{
			GetNode<SelectorBoxHandler>("./SelectionHandler"),
			GetNode<ArmyHandler>("./ArmyHandler"),
			GetNode<MapHandler>("./MapHandler"),
			GetNode<AiHandler>("./AiHandler")
		};


		foreach (var handler in allHandlers)
		{
			handler.ToGUIEvent += InvokeToGUIEvent;
			handler.ToEngineEvent += ReceiveToEngineEvent;
		}

		AllHandlersControls = new CallMulticaster(allHandlers);

		var map = GetNode<Sprite2D>("MapHandler/Map").Texture.GetImage();

		EngineState.MapInfo.Scenario.Init();

		Camera = GetNode<Camera>("./Camera");
		Camera.ZoomChanged += ViewMode;
		Camera.Reset(new Rect2(Vector2.Zero, map.GetSize()));

		GUIHandler = GetNode<GUI>("CanvasLayer/GUI");
		GUIHandler.GUIGlobalEvent += GUIEventHandler;
		GUIHandler.Init();

		AllHandlersControls.Init();
		InvokeToGUIEvent(new ToGUIUpdateCountryInfo());

		_timer = GetNode<Godot.Timer>("./DayTimer");
		


		InvokeToGUIEvent(new ToGUISetCamera(Camera, GetViewport()));
		InvokeToGUIEvent(new ToGUISetPause());
		InvokeToGUIEvent(new ToGUISetDateEvent(EngineState.MapInfo.Scenario.Date));
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
		Camera.HandleInput(@event);
		var tileId = _findTile();
		AllHandlersControls.InputHandle(@event, tileId);
		GUIHandler.InputHandle(@event);
		if (@event is InputEventMouseMotion e)
			if (tileId > -1)
			{
				InvokeToGUIEvent(new ToGUIShowInfoBoxEvent());
				GUIInfoBox.Info.ShowProvinceDataInfoBox(EngineState.MapInfo.Scenario.Map[tileId]);
			}
				
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
				_timer.WaitTime = EngineState.MapInfo.Scenario.Settings.TimeScale[e.TimeScaleId];
				return;
			case GUIPauseStateEvent e:
				if (e.IsPaused)
					_timer.Stop();
				else
					_timer.Start();
				return;
			case GUISwitchCountry e:
				EngineState.MapInfo.Scenario.AiList.Add(EngineState.PlayerCountryId);
				EngineState.MapInfo.Scenario.PlayerList.Remove(SteamState.SteamId);
				EngineState.PlayerCountryId = e.Id;
				EngineState.MapInfo.Scenario.AiList.Remove(e.Id);
				EngineState.MapInfo.Scenario.PlayerList.Add(SteamState.SteamId,e.Id);
				AllHandlersControls.GUIInteractionHandler(new GUIUpdateFogOfWar());
				InvokeToGUIEvent(new ToGUIUpdateCountryInfo());
				return;
			case GUIShowCountryWindowEvent:
				InvokeToGUIEvent(new ToGUIShowCountryWindowEvent());
				return;
			case GUIGoToProvince e:
				Camera.GoToProvince(e.Id);
				return;
			case GUIShowInfoBox e:
				InvokeToGUIEvent(new ToGUIShowInfoBoxEvent());
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
		if (!GlobalResources.MapTexture.GetUsedRect().HasPoint(iMousePos)) return -3;

		var tileId = GameMath.GetProvinceId(GlobalResources.MapTexture.GetPixelv(iMousePos));

		if (tileId < 0 || tileId >= EngineState.MapInfo.Scenario.Map.Length)
			return -3;

		return tileId;
	}
}
