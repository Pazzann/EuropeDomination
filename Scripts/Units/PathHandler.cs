using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.Units;

public partial class PathHandler : Node2D
{
	private PackedScene _pathArrowScene = (PackedScene)GD.Load("res://Prefabs/PathArrow.tscn");
	private PackedScene _curvedArrowScene = (PackedScene)GD.Load("res://Prefabs/CustomNodes/CurvedArrow.tscn");
	private ArmyUnit _armyUnit;
	private double _currentProgress;
	private int _idOfCurrentProgress;
	public  void Setup(ArmyUnit unit)
	{
		GlobalPosition = new Vector2(0, 0);
		_armyUnit = unit;
	}

	public void UpdateDayTick(ArmyUnit unit) // TODO: Move unit movement calculations to units instead of arrows
	{
		var arrow = (GetChild(0) as PathArrow);
		_currentProgress = arrow.Value;
		_idOfCurrentProgress = unit.Path[^2];
		
		var curvedArrow = GetChild(GetChildren().Count - 1) as CurvedArrow;
		curvedArrow.Value += 1f;
		
		if (!arrow.AddDay()) return;

		if (curvedArrow.RemovePoint(0))
		{
			curvedArrow.MaxValue -= (float)arrow.Value;
			curvedArrow.Value -= (float)arrow.Value;
		}

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

		List<Vector2> centres = new List<Vector2>();
		float totalDistance = 0f;
		for (int i = unit.Path.Count - 2; i > -1; i--)
		{
			centres.Add(map.Scenario.Map[unit.Path[i + 1]].CenterOfWeight);
			
			var arrow = _pathArrowScene.Instantiate() as PathArrow;
			arrow.GlobalPosition = Math.MathUtils.VectorCenter(map.Scenario.Map[unit.Path[i + 1]].CenterOfWeight, map.Scenario.Map[unit.Path[i]].CenterOfWeight) / unit.Scale;
			
			arrow.LookAt(map.Scenario.Map[unit.Path[i + 1]].CenterOfWeight / unit.Scale);
			AddChild(arrow);
			arrow.Setup(Mathf.RoundToInt((map.Scenario.Map[unit.Path[i]].CenterOfWeight - map.Scenario.Map[unit.Path[i+1]].CenterOfWeight).Length()));
			totalDistance += (map.Scenario.Map[unit.Path[i]].CenterOfWeight - map.Scenario.Map[unit.Path[i + 1]].CenterOfWeight).Length();

			if (unit.Path[^2] == _idOfCurrentProgress && unit.Path.Count - 2 == i)
			{
				arrow.Value = _currentProgress;
			}
		}
		centres.Add(map.Scenario.Map[unit.Path[0]].CenterOfWeight);

		var curvedArrow = _curvedArrowScene.Instantiate() as CurvedArrow;
		AddChild(curvedArrow);
		curvedArrow.Setup(centres);
		curvedArrow.DrawLine();
		curvedArrow.MaxValue = totalDistance;
	}
}
