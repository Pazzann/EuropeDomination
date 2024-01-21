using EuropeDominationDemo.Scripts.Scenarios;
using Godot;

namespace EuropeDominationDemo.Scripts;

public partial class ArmyUnit : Node2D
{
	public int UnitId; 
	public ArmyUnitData Data;
	private MapHandler _mapHandler; 

	private Button _selectButton;
	private Sprite2D _selectionSprite;
	private AnimatedSprite2D _armyUnitSprite;
	

	public override void _Ready()
	{
		_selectButton = GetChild(0) as Button;
		_selectionSprite = GetChild(1) as Sprite2D;
		_armyUnitSprite = GetChild(2) as AnimatedSprite2D;
	}

	public void SetupUnit(int unitId, ArmyUnitData armyUnitData, MapHandler mapHandler)
	{
		UnitId = unitId;
		Data = armyUnitData;
	
		_mapHandler = mapHandler;
	}
	
	
	private void _on_button_pressed()
	{
		_selectionSprite.Visible = !_selectionSprite.Visible;
	}

}

