using EuropeDominationDemo.Scripts.GlobalStates;
using Godot;
using Steamworks;

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

	private async void _onCreateLobbyButtonPressed()
	{
		
		GD.Print("Create Lobby Pressed");
		
		var data = await SteamMatchmaking.CreateLobbyAsync(16);
		MultiplayerState.MultiplayerMode = true;
		MultiplayerState.Lobby = data;
		MultiplayerState.Lobby?.SetPublic();
		MultiplayerState.Lobby?.SetJoinable(true);
		MultiplayerState.Lobby?.SetData("name", "Test Lobby");
		var lobbyMembers = MultiplayerState.Lobby?.Members;
		
		/*var logo  =  await data?.Owner.GetMediumAvatarAsync();
		var logoGodot = new Texture2D();
		//ImageTexture.CreateFromImage(Image.CreateFromData(logo?.Width ?? 0, logo?.Height,  true, Image.Format.Rgb8, logo?.Data))
		GD.Print();
		*/
		GetTree().ChangeSceneToFile("res://Scenes/LobbyScene.tscn");
		
	}
	
	

	#endregion
}
