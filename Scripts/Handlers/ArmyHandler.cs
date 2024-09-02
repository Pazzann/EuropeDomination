using Godot;
using System;
using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Handlers;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Units;

public partial class ArmyHandler : GameHandler
{
	private PackedScene _armyScene;
	private bool _isShiftPressed;


	public override void Init()
	{
		_armyScene = (PackedScene)GD.Load("res://Prefabs/ArmyUnit.tscn");
		
		foreach (var data in EngineState.MapInfo.Scenario.ArmyUnits)
		{

			ArmyUnit obj = _armyScene.Instantiate() as ArmyUnit;
			obj.SetupUnit(data, EngineState.MapInfo);


			//TODO: NORMAL CALCULATION OF ARMY POSITION
			obj.Position = new Vector2(EngineState.MapInfo.Scenario.Map[data.CurrentProvince].CenterOfWeight.X + 5,EngineState.MapInfo.Scenario.Map[data.CurrentProvince].CenterOfWeight.Y);
			AddChild(obj);
		}
	}

	public override bool InputHandle(InputEvent @event, int tileId)
	{
		if (@event is InputEventKey { KeyLabel: Key.Shift, Pressed: true })
			_isShiftPressed = true;
		if (@event is InputEventKey { KeyLabel: Key.Shift, Pressed: false })
			_isShiftPressed = false;
		
		if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Right, Pressed: true })
		{
			if (tileId == -3)
				return false;

			if (!_isShiftPressed)
				foreach (var armyUnit in EngineState.MapInfo.CurrentSelectedUnits)
				{
					if (tileId == armyUnit.Data.CurrentProvince)
					{
						armyUnit.NewPath(Array.Empty<int>());
						return false;
					}

					armyUnit.NewPath(
						PathFinder.FindPathFromAToB(armyUnit.Data.CurrentProvince, tileId,  EngineState.MapInfo.Scenario.Map));
				}
			else
				foreach (var armyUnit in EngineState.MapInfo.CurrentSelectedUnits)
				{
					if (armyUnit.Path.Count == 0)
					{
						armyUnit.NewPath(PathFinder.FindPathFromAToB(armyUnit.Data.CurrentProvince, tileId,  EngineState.MapInfo.Scenario.Map));
						return false;
					}
					armyUnit.AddPath(
						PathFinder.FindPathFromAToB(armyUnit.Path[0], tileId, EngineState.MapInfo.Scenario.Map));
				}

			return false;
		}
		return false;
	}

	public override void ViewModUpdate(float zoom)
	{
		if (EngineState.MapInfo.CurrentMapMode == MapTypes.Political && zoom > 3.0f)
		{
			Visible = true;
			return;
		}

		Visible = false;
	}

	public override void GUIInteractionHandler(GUIEvent @event)
	{
		switch (@event)
		{
			default:
				return;
		}
	}

	public override void DayTick()
	{
		foreach (var armyUnit in GetChildren())
		{
			(armyUnit as ArmyUnit).UpdateDayTick();
		}
	}

	public override void MonthTick()
	{
		
	}

	public override void YearTick()
	{
		
	}
}
