using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIPrefabs;

public delegate void LoadButtonPressedDelegate(string loadName, bool isScenario);

public partial class GuiLoadPanel : PanelContainer
{
	public event LoadButtonPressedDelegate LoadButtonPressedEvent;

	private VBoxContainer _savesContainer;
	private VBoxContainer _scenariosContainer;
	private PackedScene _loadPrefab;
	
	public override void _Ready()
	{
		_scenariosContainer = 
			GetNode<VBoxContainer>("MarginContainer/VBoxContainer/TabContainer/Scenarios/ScrollContainer/ScenariosContainer");
		_savesContainer =
			GetNode<VBoxContainer>("MarginContainer/VBoxContainer/TabContainer/Saves/ScrollContainer2/SavesContainer");
		_loadPrefab = GD.Load<PackedScene>("res://Prefabs/ScenesPrefabs/ChooseSave.tscn");
		UpdateSaves();
	}

	public void UpdateSaves()
	{
		foreach (var scenario in _scenariosContainer.GetChildren())
			scenario.QueueFree();
		foreach (var save in _savesContainer.GetChildren())
			save.QueueFree();

		foreach (var save in SaveLoadGamesUtils.GetSavesList())
		{
			var a = _loadPrefab.Instantiate();
			a.GetChild(0).GetChild(0).GetChild<Label>(0).Text = save;
			a.GetChild(0).GetChild(0).GetChild<Button>(1).Pressed += () => LoadButtonPressedEvent.Invoke(save, false);
			_savesContainer.AddChild(a);
		}
		foreach (var scenario in SaveLoadGamesUtils.GetScenariosList())
		{
			var a = _loadPrefab.Instantiate();
			a.GetChild(0).GetChild(0).GetChild<Label>(0).Text = scenario;
			a.GetChild(0).GetChild(0).GetChild<Button>(1).Pressed += () => LoadButtonPressedEvent.Invoke(scenario, true);
			_scenariosContainer.AddChild(a);
		}
	}

}
