using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUIEscapeMenu : GUIHandler
{

	private Button _loadGameButton;
	public override void Init()
	{
		_loadGameButton = GetNode<Button>("ButtonPanel/MarginContainer/VBoxContainer/VBoxContainer/Load");
		_loadGameButton.Visible = !MultiplayerState.MultiplayerMode;
	}

	public override void InputHandle(InputEvent @event)
	{
		if (Input.IsActionJustReleased("escape_menu"))
		{
			Visible = !Visible;
		}
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
	}

	private void _onSaveButtonPressed()
	{
		
	}

	private void _onLoadButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/LobbyScene.tscn");
	}

	private void _onSettingsButtonPressed()
	{
		
	}

	private void _onBackButtonPressed()
	{
		Visible = false;
	}

	private void _onExitToMenuButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://main.tscn");
	}

	private void _onExitToDesktopButtonPressed()
	{
		GetTree().Quit();
	}

	private void _onFinalSavePressed()
	{
		SaveLoadGamesUtils.SaveGame("exp", EngineState.MapInfo.Scenario);
	}
}
