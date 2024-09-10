using Godot;
using System;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUIBattleWindow : GUIHandler
{
	private BattleData _currentlyShownBattleData;
	private VBoxContainer _battleMatrixContainer;
	public override void Init()
	{
		_battleMatrixContainer = GetNode<VBoxContainer>("PanelContainer/MarginContainer/VBoxContainer");
		for (int i = 0; i < 20; i++)
		{
			for (int j = 0; j < 20; j++)
			{
				var coords = new Vector2I(i, j);
				_battleMatrixContainer.GetChild(i).GetChild<ColorRect>(j).MouseEntered += () => _updateInfoBox(coords);
			}
		}
		
	}

	public override void InputHandle(InputEvent @event)
	{
		
	}

	private void _updateInfoBox(Vector2I coords)
	{
		InvokeGUIEvent(new GUIShowInfoBox(InfoBoxFactory.BattleRegimentData(_currentlyShownBattleData.Battlefield[coords.X, coords.Y] as ArmyRegiment)));
	}

	private void _showData()
	{
		for (int i = 0; i < _currentlyShownBattleData.Battlefield.GetLength(0); i++) 
		{ 
			for (int j = 0; j < _currentlyShownBattleData.Battlefield.GetLength(1); j++)
			{
				Color color;
				switch (_currentlyShownBattleData.Battlefield[i, j])
				{
					case ArmyInfantryRegiment d:
						color = (_currentlyShownBattleData.Attacker as ArmyUnitData).Regiments.Contains(d)? MapDefaultColors.AttackerInfantryInBattle : MapDefaultColors.DefenderInfantryInBattle;
						break;
					case ArmyArtilleryRegiment d:
						color = (_currentlyShownBattleData.Attacker as ArmyUnitData).Regiments.Contains(d)? MapDefaultColors.AttackerCavalryInBattle : MapDefaultColors.DefenderCavalryInBattle;
						break;
					case ArmyCavalryRegiment d:
						color = (_currentlyShownBattleData.Attacker as ArmyUnitData).Regiments.Contains(d)? MapDefaultColors.AttackerArtilleryInBattle : MapDefaultColors.DefenderArtilleryInBattle;
						break;
					default:
						color = MapDefaultColors.EmptyInBattle;
						break;
				}
				_battleMatrixContainer.GetChild(i).GetChild<ColorRect>(j).Color = color;
			} 
		}  
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGUIShowBattleData e:
			{
				_currentlyShownBattleData = e.BattleData;
				_showData();
				Visible = true;
				return;
			} 
		}	
	}
}
