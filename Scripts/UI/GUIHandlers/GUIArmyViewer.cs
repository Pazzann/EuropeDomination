using System.Collections.Generic;
using Godot;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;
using EuropeDominationDemo.Scripts.Units;

public partial class GUIArmyViewer : GUIHandler
{
	private PackedScene _armyWindowScene;
	private List<ArmyUnit> _currentlyShownArmyUnits;
	private VBoxContainer _armyWindowSpawner;
	
	public override void Init()
	{
		_armyWindowScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIArmyWindow.tscn");
		_armyWindowSpawner = GetNode<VBoxContainer>("PanelContainer/VBoxContainer/ScrollContainer/VBoxContainer");
	}

	public override void InputHandle(InputEvent @event)
	{
		
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGUIEventShowArmyViewerEvent e:
			{
				_currentlyShownArmyUnits = e.ArmyUnits;
				_setInfo();
				Visible = true;
				return;
			}
			case ToGUIEventHideArmyViewerEvent:
			{
				Visible = false;
				return;
			}
			default:
				return;
		}
	}

	private void _setInfo()
	{
		foreach (var child in _armyWindowSpawner.GetChildren())
		{
			child.QueueFree();	
		}

		foreach (var armyUnit in _currentlyShownArmyUnits)
		{
			var window = _armyWindowScene.Instantiate() as GUIArmyWindow;
			_armyWindowSpawner.AddChild(window);
		}
		
	}
}
