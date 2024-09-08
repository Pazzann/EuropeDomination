using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Scenarios.DiplomacyAgreements;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;
using Godot.Collections;

namespace EuropeDominationDemo.Scripts.Units;

public partial class ArmyUnit : Node2D
{
	public ArmyUnitData Data;
	private MapData _mapData;


	private AnimatedSprite2D _armyUnitSprite;

	private PathHandler _pathHandler;

	//fix this govnocode
	private ArmyHandler _armyArmyHandler;

	private bool _isSelected = false;

	public bool IsSelected
	{
		get => _isSelected;
		set
		{
			if (value)
				_armyUnitSprite.SelfModulate = new Color(0, 1, 0);
			else
				_armyUnitSprite.SelfModulate = new Color(1, 1, 1);
			_pathHandler.Visible = value;
		}
	}

	private List<KeyValuePair<int, int>> _evaluatePath(List<int> path)
	{
		var a = new List<KeyValuePair<int, int>>();
		path.Reverse();
		for (int i = 0; i < path.Count - 1; i++)
		{
			a.Add(new KeyValuePair<int, int>(path[i],
				Mathf.RoundToInt((EngineState.MapInfo.Scenario.Map[path[i]].CenterOfWeight -
								  EngineState.MapInfo.Scenario.Map[path[i + 1]].CenterOfWeight).Length())));
		}

		a.Add(new KeyValuePair<int, int>(path[^1], 0));
		a.Reverse();
		return a;
	}

	public void AddPath(int[] path)
	{
		var prev = Data.MovementQueue;
		Data.MovementQueue = new List<KeyValuePair<int, int>>();
		Data.MovementQueue.AddRange(_evaluatePath(path.Take(path.Count() - 1).ToList()));
		prev[0] = new KeyValuePair<int, int>(prev[0].Key,
			Mathf.RoundToInt((EngineState.MapInfo.Scenario.Map[prev[0].Key].CenterOfWeight -
							  EngineState.MapInfo.Scenario.Map[Data.MovementQueue[^1].Key].CenterOfWeight).Length()));
		Data.MovementQueue.AddRange(prev);
		_pathHandler.DrawArrows(this);
	}


	public void NewPath(int[] path)
	{
		Data.MovementQueue = new List<KeyValuePair<int, int>>();
		Data.MovementQueue.AddRange(_evaluatePath(path.ToList()));
		_pathHandler.DrawArrows(this);
	}

	public void UpdateDayTick()
	{
		if (Data.MovementQueue.Count is 1 or 0)
			return;
		var previousLength = Data.MovementQueue.Count;
		_pathHandler.UpdateDayTick(this);
		if (Data.MovementQueue.Count < previousLength)
		{
			_pathHandler.MoveArrows(Scale, GlobalPosition,
				_mapData.Scenario.Map[Data.MovementQueue[^1].Key].CenterOfWeight);
			GlobalPosition = _mapData.Scenario.Map[Data.MovementQueue[^1].Key].CenterOfWeight;
			Data.CurrentProvince = Data.MovementQueue[^1].Key;
			if (Data.MovementQueue.Count is 1)
				Data.MovementQueue = new List<KeyValuePair<int, int>>();

			var countriesWithOnGoingWars = EngineState.MapInfo.Scenario.Countries.Where(d =>
				d.Value.DiplomacyAgreements.Any(a => a.Key == Data.Owner && a.Value.Any(g => g is War)));
			if (!countriesWithOnGoingWars.Any()) return;

			var countriesWithUnitsIntheSameProvince = countriesWithOnGoingWars.Where(d =>
				d.Value.Units.Any(a => a.CurrentProvince == Data.CurrentProvince));
			if (!countriesWithUnitsIntheSameProvince.Any()) return;

			var unitsInThisProvince = new List<UnitData>();
			foreach (var data in countriesWithUnitsIntheSameProvince)
			{
				unitsInThisProvince.AddRange(data.Value.Units.Where(d => d.CurrentProvince == Data.CurrentProvince));
			}

			if (unitsInThisProvince.Count > 0)
			{
				var battle = new BattleData(unitsInThisProvince[0] as ArmyUnitData, Data);
				unitsInThisProvince[0].UnitState = UnitStates.Battle;
				Data.UnitState = UnitStates.Battle;
				EngineState.MapInfo.Scenario.Battles.Add(battle);
				_armyArmyHandler.InvokeToGUIEvent(new ToGUIShowBattleData(battle));
			}
		}
	}

	public override void _Ready()
	{
		_armyUnitSprite = GetChild(0) as AnimatedSprite2D;
		_pathHandler = GetChild(1) as PathHandler;
		_pathHandler.Setup(this);
	}

	public void SetupUnit(ArmyUnitData armyUnitData, MapData mapData, ArmyHandler armyHandler)
	{
		Data = armyUnitData;
		_armyArmyHandler = armyHandler;
		_mapData = mapData;
	}

	public bool IsInsideRect(Rect2 rect)
	{
		Vector2 spriteSize = _armyUnitSprite.SpriteFrames
			.GetFrameTexture(_armyUnitSprite.Animation, _armyUnitSprite.Frame).GetSize();
		Rect2 thisRect = new Rect2((GlobalPosition - spriteSize * Scale / 2), spriteSize * Scale);
		return rect.Intersects(thisRect);
	}

	public static void SelectUnits(Array<Node> allUnits, List<ArmyUnit> selectedUnits)
	{
		foreach (var armyUnit in allUnits)
		{
			((ArmyUnit)armyUnit).IsSelected = false;
		}

		foreach (var armyUnit in selectedUnits)
		{
			armyUnit.IsSelected = true;
		}
	}
}
