using EuropeDominationDemo.Scripts.GlobalStates;
using Godot;
using Steamworks;
using Steamworks.Data;

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
		SteamFriends.OnGameLobbyJoinRequested += _onJoinLobbyRequest;
	}

	private void _onExitPressed()
	{
		GetTree().Quit();
	}
	private void _onSinglePlayerPressed()
	{
		MultiplayerState.MultiplayerMode = false;
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

	private async void _onJoinLobbyRequest(Lobby joinedLobby, SteamId steamId)
	{
		RoomEnter enteredLobby = await joinedLobby.Join();
		if(enteredLobby != RoomEnter.Success)
			return;
		MultiplayerState.MultiplayerMode = true;
		MultiplayerState.Lobby = joinedLobby;
		GetTree().ChangeSceneToFile("res://Scenes/LobbyScene.tscn");
		
	}
	private async void _onCreateLobbyButtonPressed()
	{
		
		GD.Print("Create Lobby Pressed");
		
		var data = await SteamMatchmaking.CreateLobbyAsync(16);
		MultiplayerState.MultiplayerMode = true;
		MultiplayerState.Lobby = data;
		MultiplayerState.Lobby?.SetPublic();
		MultiplayerState.Lobby?.SetJoinable(true);
		MultiplayerState.Lobby?.SetData("name", "Test Lobby");
		
		GetTree().ChangeSceneToFile("res://Scenes/LobbyScene.tscn");
		
	}
	
	

	#endregion
}
