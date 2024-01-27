using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EuropeDominationDemo.Scripts.Scenarios;
using Godot;
using Godot.Collections;

namespace EuropeDominationDemo.Scripts.Units;

public partial class ArmyUnit : Node2D
{
	public ArmyUnitData Data;
	private MapData _mapData; 
	
	private Sprite2D _selectionSprite;
	private AnimatedSprite2D _armyUnitSprite;

	private PathHandler _pathHandler;

	public List<int> Path = new List<int>();

	private bool _isSelected = false;
	public bool IsSelected
	{
		get => _isSelected;
		set
		{ 
			_selectionSprite.Visible = value;
			_pathHandler.Visible = value;
			_isSelected = value;
		} 
	}

	public void AddPath(int[] path)
	{
		var prev = Path;
		path = path.Take(path.Count() - 1).ToArray();
		Path = new List<int>();
		Path.AddRange(path);
		Path.AddRange(prev);
		_pathHandler.DrawArrows(this, _mapData);
	}

	public void NewPath(int[] path)
	{
		Path = new List<int>();
		Path.AddRange(path);
		_pathHandler.DrawArrows(this, _mapData);
	}

	public void UpdateDayTick()
	{
		if (Path.Count is 1 or 0)
			return;
		var previousLength = Path.Count;
		_pathHandler.UpdateDayTick(this);
		if (Path.Count < previousLength)
		{
			_pathHandler.MoveArrows(Scale, GlobalPosition, _mapData.Scenario.Map[Path[^1]].CenterOfWeight);
			GlobalPosition = _mapData.Scenario.Map[Path[^1]].CenterOfWeight;
			Data.CurrentProvince = Path[^1];
			if (Path.Count is 1)
				Path = new List<int>();
		}
			
	}
	
	public override void _Ready()
	{
		_selectionSprite = GetChild(0) as Sprite2D;
		_armyUnitSprite = GetChild(1) as AnimatedSprite2D;
		_pathHandler = GetChild(2) as PathHandler;
		_pathHandler.Setup(this);
	}

	public void SetupUnit(ArmyUnitData armyUnitData, MapData mapData)
	{

		Data = armyUnitData;
	
		_mapData = mapData;
	}

	public bool IsInsideRect(Rect2 rect)
	{
		Vector2 spriteSize = _armyUnitSprite.SpriteFrames.GetFrameTexture(_armyUnitSprite.Animation, _armyUnitSprite.Frame).GetSize();
		Rect2 thisRect = new Rect2((GlobalPosition - spriteSize * Scale / 2) , spriteSize * Scale);
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

