using Godot;

namespace EuropeDominationDemo.Scripts.Scenes;

public partial class MainScene : TextureRect
{
	private PanelContainer _multiplayerPanel;
	private TabContainer _settingsPanel;
	private PackedScene _lobbyScene;
	public override void _Ready()
	{
		_multiplayerPanel = GetNode<PanelContainer>("MultiplayerLobbys");
		_settingsPanel = GetNode<TabContainer>("MainSettings");
		_lobbyScene = GD.Load<PackedScene>("res://Scenes/LobbyScene.tscn");
	}

	private void _onSinglePlayerPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/LobbyScene.tscn");
	}

	private void _onMultiPlayerPressed()
	{
		_multiplayerPanel.Visible = true;
	}
	
	private void _onSettingsPressed()
	{
		
	}

	#region Multiplayer lobbies

	

	#endregion
}
