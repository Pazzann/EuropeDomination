using Godot;
using System;
using System.Collections.Generic;
using EuropeDominationDemo.Scripts;
using EuropeDominationDemo.Scripts.Scenarios;
using Microsoft.VisualBasic.CompilerServices;
using Utils = EuropeDominationDemo.Scripts.Math.Utils;

public partial class PathHandler : Node2D
{
	private PackedScene _pathArrowScene = (PackedScene)GD.Load("res://Prefabs/PathArrow.tscn");
	private ArmyUnit _armyUnit;
	private double _currentProgress;
	private int _idOfCurrentProgress;
	public  void Setup(ArmyUnit unit)
	{
		GlobalPosition = new Vector2(0, 0);
		_armyUnit = unit;
	}

	public void UpdateDayTick(ArmyUnit unit)
	{
		var arrow = (GetChild(0) as PathArrow);
		_currentProgress = arrow.Value;
		_idOfCurrentProgress = unit.Path[^2];
		if (!arrow.AddDay()) return;
		unit.Path.Remove(unit.Path[^1]);
		arrow.QueueFree();
	}

	public void MoveArrows(Vector2 scale, Vector2 prev, Vector2 next)
	{
		Position -= (next - prev) / scale;
	}

	public void DrawArrows(ArmyUnit unit, MapData map)
	{
		GlobalPosition = Vector2.Zero;
		foreach (var pathArrow in GetChildren())
		{
			pathArrow.QueueFree();
		}
		for (int i = unit.Path.Count - 2; i > -1; i--)
		{
			var arrow = _pathArrowScene.Instantiate() as PathArrow;
			arrow.GlobalPosition = Utils.VectorCenter(map.Scenario.Map[unit.Path[i + 1]].CenterOfWeight, map.Scenario.Map[unit.Path[i]].CenterOfWeight) / unit.Scale;
			
			arrow.LookAt(map.Scenario.Map[unit.Path[i + 1]].CenterOfWeight / unit.Scale);
			AddChild(arrow);
			arrow.Setup(Mathf.RoundToInt((map.Scenario.Map[unit.Path[i]].CenterOfWeight - map.Scenario.Map[unit.Path[i+1]].CenterOfWeight).Length()));

			if (unit.Path[^2] == _idOfCurrentProgress && unit.Path.Count - 2 == i)
			{
				arrow.Value = _currentProgress;
			}
		}
	}
}
