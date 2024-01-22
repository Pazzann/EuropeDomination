using System.Collections.Generic;
using System.Linq.Expressions;
using EuropeDominationDemo.Scripts.Scenarios;
using Godot;
using Godot.Collections;

namespace EuropeDominationDemo.Scripts;

public partial class ArmyUnit : Node2D
{
	public int UnitId; 
	public ArmyUnitData Data;
	private MapHandler _mapHandler; 
	
	private Sprite2D _selectionSprite;
	private AnimatedSprite2D _armyUnitSprite;


	private bool _isSelected = false;
	public bool IsSelected
	{
		get => _isSelected;
		set
		{ 
			_selectionSprite.Visible = value;
			_isSelected = value;
		} 
	}


	public override void _Ready()
	{
		_selectionSprite = GetChild(0) as Sprite2D;
		_armyUnitSprite = GetChild(1) as AnimatedSprite2D;
	}

	public void SetupUnit(int unitId, ArmyUnitData armyUnitData, MapHandler mapHandler)
	{
		UnitId = unitId;
		Data = armyUnitData;
	
		_mapHandler = mapHandler;
	}

	public bool IsInsideRect(Rect2 rect)
	{
		Vector2 spriteSize = _armyUnitSprite.SpriteFrames.GetFrameTexture(_armyUnitSprite.Animation, _armyUnitSprite.Frame).GetSize();
		Rect2 thisRect = new Rect2((GlobalPosition - spriteSize / 2) * Scale, spriteSize);
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
			((ArmyUnit)armyUnit).IsSelected = true;
		}
	}
	
}

