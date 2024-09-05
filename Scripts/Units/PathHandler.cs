using System.Collections.Generic;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.Units;

public partial class PathHandler : Node2D
{
	private PackedScene _curvedArrowScene = (PackedScene)GD.Load("res://Prefabs/CustomNodes/CurvedArrow.tscn");
	private ArmyUnit _armyUnit;
	public  void Setup(ArmyUnit unit)
	{
		GlobalPosition = new Vector2(0, 0);
		_armyUnit = unit;
	}

	public void UpdateDayTick(ArmyUnit unit) 
	{
		var curvedArrow = GetChild(GetChildren().Count - 1) as CurvedArrow;
		curvedArrow.Value += 1f;
		
		if (!_armyUnit.Data.AddDay()) return;

		if (curvedArrow.RemovePoint(0))
		{
			curvedArrow.MaxValue -= (float)unit.Data.MovementProgress;
			curvedArrow.Value -= (float)unit.Data.MovementProgress;
		}

		unit.Data.MovementQueue.Remove(unit.Data.MovementQueue[^1]);
		unit.Data.MovementProgress = 0;

	}

	public void MoveArrows(Vector2 scale, Vector2 prev, Vector2 next)
	{
		Position -= (next - prev) / scale;
	}

	public void DrawArrows(ArmyUnit unit)
	{
		GlobalPosition = Vector2.Zero;
		var curvedArrow = _curvedArrowScene.Instantiate() as CurvedArrow;
		AddChild(curvedArrow);
		curvedArrow.Setup(_getCentersList(unit.Data));
		curvedArrow.DrawLine();
		curvedArrow.MaxValue = unit.Data.TotalDistance;
	}

	private List<Vector2> _getCentersList(UnitData data)
	{
		var a = new List<Vector2>();
		foreach (var pair in data.MovementQueue)
		{
			a.Add(EngineState.MapInfo.Scenario.Map[pair.Key].CenterOfWeight);
		}

		a.Reverse();
		return a;
	}
}
