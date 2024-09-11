using System.Collections.Generic;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;
using EuropeDominationDemo.Scripts.Units;
using Godot;

public partial class GUIArmyViewer : GUIHandler
{
	private PackedScene _armyWindowScene;
	private VBoxContainer _armyWindowSpawner;
	private List<ArmyUnit> _currentlyShownArmyUnits;

	public override void Init()
	{
		_armyWindowScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIArmyWindow.tscn");
		_armyWindowSpawner = GetNode<VBoxContainer>("PanelContainer/VBoxContainer/ScrollContainer/VBoxContainer");
	}

	public override void InputHandle(InputEvent @event)
	{
	}

	private void _onMergePressed()
	{
		InvokeGUIEvent(new GUIMergeUnitsEvent(_currentlyShownArmyUnits));
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGUIShowArmyViewerEvent e:
			{
				_currentlyShownArmyUnits = e.ArmyUnits;
				_setInfo();
				Visible = true;
				return;
			}
			case ToGUIHideArmyViewerEvent:
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
		foreach (var child in _armyWindowSpawner.GetChildren()) child.QueueFree();

		foreach (var armyUnit in _currentlyShownArmyUnits)
		{
			var window = _armyWindowScene.Instantiate() as GUIArmyWindow;
			window.ShowInfo(armyUnit.Data);
			if (_currentlyShownArmyUnits.Count > 1) window.HideHalfInfo();
			_armyWindowSpawner.AddChild(window);
		}
	}
}
