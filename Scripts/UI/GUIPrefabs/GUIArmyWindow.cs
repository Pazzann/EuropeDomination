using EuropeDominationDemo.Scripts.Scenarios.Army;
using Godot;

public partial class GUIArmyWindow : VBoxContainer
{
	private PackedScene _regimentInfoScene;

	public void ShowInfo(ArmyUnitData armyUnitData)
	{
		var regimentContainer =
			GetNode<VBoxContainer>(
				"VBoxContainer/AdditionalInfo/Regiments/MarginContainer/ScrollContainer/ArmyRegimentContainer");
		foreach (var unit in regimentContainer.GetChildren()) unit.QueueFree();

		_regimentInfoScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIRegimentInfo.tscn");
		foreach (var regiment in armyUnitData.Regiments)
		{
			var a = _regimentInfoScene.Instantiate();
			regimentContainer.AddChild(a);
		}
	}

	public void HideHalfInfo()
	{
		//GetNode<HBoxContainer>("VBoxContainer/AdditionalInfo").Visible = false;
	}
}
